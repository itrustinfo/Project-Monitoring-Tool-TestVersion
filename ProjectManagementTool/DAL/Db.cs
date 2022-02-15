using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Db
/// </summary>
public static class Db
{
    public static DataTable GetTasks(string parent)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Tasks] WHERE [ParentTaskID] = @parent and StartDate<>'' and ActualEndDate<>'' order by [StartDate], [Name] ASC", ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString);
        if (parent == null)
        {
            da.SelectCommand.CommandText = da.SelectCommand.CommandText.Replace("= @parent", "is null");
        }
        else
        {
            da.SelectCommand.Parameters.AddWithValue("parent", parent);
        }

        DataTable dt = new DataTable();
        da.Fill(dt);

        return dt;
    }

    public static DataTable GetTasksFlat(Guid ProjectUID,Guid WorkPAckageUID)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Tasks] where ProjectUID='" + ProjectUID + "' and WorkPackageUID='" + WorkPAckageUID + "' and StartDate<>'' and ActualEndDate<>'' and TaskLevel=1 order by [StartDate], [Name] ASC", ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString);
        DataTable dt = new DataTable();
        da.Fill(dt);

        return dt;
    }

    public static DataRow GetTask(string id)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [Tasks] where StartDate<>'' and ActualEndDate<>'' WHERE [TaskUID] = '" + id + "'", ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString);
        //da.SelectCommand.Parameters.AddWithValue("TaskUID", id);

        DataTable dt = new DataTable();
        da.Fill(dt);

        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0];
        }
        return null;
    }

    public static DataTable GetLinks(Guid ProjectUID)
    {
      //  SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM [TaskLinks] where ProjectUID='" + ProjectUID + "'", ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString);

        DataTable dt = new DataTable();
        //da.Fill(dt);

        return dt;
    }

    public static int InsertTask(DateTime start, DateTime end, string name, string parent, int ordinal)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO [task] ([start], [end], [name], [parent_id], [ordinal], [ordinal_priority]) VALUES(@start, @end, @name, @parent, @ordinal, @priority)", con);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("parent", (object)parent ?? DBNull.Value);
            cmd.Parameters.AddWithValue("ordinal", ordinal);
            cmd.Parameters.AddWithValue("priority", DateTime.Now);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("select @@identity;", con);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            return id;
        }
    }

    public static int InsertMilestone(DateTime start, string name, string parent, int ordinal)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO [task] ([start], [end], [milestone], [name], [parent_id], [ordinal], [ordinal_priority]) VALUES(@start, @end, @milestone, @name, @parent, @ordinal, @priority)", con);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", start);
            cmd.Parameters.AddWithValue("milestone", true);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("parent", (object)parent ?? DBNull.Value);
            cmd.Parameters.AddWithValue("ordinal", ordinal);
            cmd.Parameters.AddWithValue("priority", DateTime.Now);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("select @@identity;", con);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            return id;
        }
    }

    public static int InsertLink(string from, string to, string type)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO [link] ([from], [to], [type]) VALUES(@from, @to, @type)", con);
            cmd.Parameters.AddWithValue("from", from);
            cmd.Parameters.AddWithValue("to", to);
            cmd.Parameters.AddWithValue("type", type);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("select @@identity;", con);
            int id = Convert.ToInt32(cmd.ExecuteScalar());
            return id;
        }
    }

    public static void UpdateTask(string id, DateTime start, DateTime end)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [task] SET [start] = @start, [end] = @end WHERE [id] = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("start", start);
            cmd.Parameters.AddWithValue("end", end);
            cmd.ExecuteNonQuery();
        }
    }

    public static void UpdateTaskParent(string id, string parent, int ordinal)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [task] SET [parent_id] = @parent, [ordinal] = @ordinal, [ordinal_priority] = @priority WHERE [id] = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("parent", (object)parent ?? DBNull.Value);
            cmd.Parameters.AddWithValue("ordinal", ordinal);
            cmd.Parameters.AddWithValue("priority", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
    }



    public static void ClearEvents()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["daypilot"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from [task]", con);
            cmd.ExecuteNonQuery();
        }
    }

    public static void CompactOrdinals(string parent)
    {
        DataTable children = GetTasks(parent);

        int i = 0;
        foreach (DataRow row in children.Rows)
        {
            DbUpdateTaskOrdinal(Convert.ToString(row["id"]), i);
            i += 1;
        }
    }

    public static void DbUpdateTaskOrdinal(string id, int ordinal)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [task] SET [ordinal] = @ordinal, [ordinal_priority] = @priority WHERE [id] = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("ordinal", ordinal);
            cmd.Parameters.AddWithValue("priority", DateTime.Now);
            cmd.ExecuteNonQuery();
        }
    }

    public static void DeleteTaskWithChildren(string id)
    {
        DataTable children = GetTasks(id);
        foreach (DataRow child in children.Rows)
        {
            DeleteTaskWithChildren(Convert.ToString(child["id"]));
        }

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE from [task] WHERE [id] = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public static int GetMaxOrdinal(string parent)
    {
        SqlDataAdapter da = new SqlDataAdapter("SELECT max(ordinal) FROM [task] WHERE [parent_id] = @parent", ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString);
        if (parent == null)
        {
            da.SelectCommand.CommandText = da.SelectCommand.CommandText.Replace("= @parent", "is null");
        }
        else
        {
            da.SelectCommand.Parameters.AddWithValue("parent", parent);
        }

        DataTable dt = new DataTable();
        da.Fill(dt);

        object result = dt.Rows[0][0];

        if (result is DBNull)
        {
            return 0;
        }

        return (int)result;
    }



    public static void ToMilestone(string id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [task] SET [milestone] = 1 WHERE [id] = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
        
    }

    public static void ToTask(string id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [task] SET [milestone] = 0 WHERE [id] = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
        
    }

    public static void DeleteLink(string id)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("DELETE from [link] WHERE [id] = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.ExecuteNonQuery();
        }
    }

    public static void UpdateTaskNameComplete(string id, string name, int complete)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["PMConnectionString"].ConnectionString))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE [task] SET [name] = @name, [complete] = @complete WHERE [id] = @id", con);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("complete", complete);
            cmd.ExecuteNonQuery();
        }

    }


}