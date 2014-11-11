using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Facebook;
using System.Text.RegularExpressions;
namespace faceForm
{
    public partial class Form1 : Form
    {
        public static String AccessToken = @"CAACEdEose0cBALVigLJcW1QQHXiuTmlBFtB6XOF5oZBN1nBR7pr1R5Ez915Q7JqU5WOOwZBugv0EZA62rxUcNZAKgVvvOJfPWIAlOtIszU8FCZAEoiBIpVNuV9AMyNHbpKq9lOBMr29vYntfpSiLbG358O2NkLZCc4yVZCOt8jJGzHQi6e7hzODasrZC7GDJ5RIZAPYhzbEAknB2sd3KwZAGMwrDIIg1nnHTEZD"; 
        
        string connetionString = null;
        SqlConnection sqlCnn;
        SqlCommand sqlCmd, cmd;
        SqlDataAdapter adapter = new SqlDataAdapter();
        String[] about_arr = new String[500];
        List<String []> arrList = new List<String []>();
        List<String> final_big_unique;
        //List<string,string> lst=
        List<String> about_lst = new List<string>();
        List<String []> new_unique_lst;
       // DataSet ds = new DataSet();
        String about, category, location_street, location_city, username, id, pagename,link;
        List<string> posts=new List<string>();

       // UTF8Encoding utf8 = new UTF8Encoding();
       public  static string[] stop_wrd = System.IO.File.ReadAllLines(@"D:\New folder (4)\StopWords.txt");

       public static string[] stop_chars = System.IO.File.ReadAllLines(@"D:\New folder (4)\StopCharacters.txt");
        

        public static dynamic GetPageInformation(string pageFBIdOrName)
        {
                FacebookClient Client = new FacebookClient(AccessToken);


             dynamic PageInfo = Client.Get(@"https://graph.facebook.com/" + pageFBIdOrName);
             return PageInfo;


        }

        public static dynamic getPosts(string pageFBIdOrName) { 
        
        
                FacebookClient Client = new FacebookClient(AccessToken);


             dynamic PostInfo = Client.Get(@"https://graph.facebook.com/" + pageFBIdOrName+"?fields=posts{message}");
             return PostInfo;

        
        }



        //////////////


        public void page_info(String page_username)
        {
            dynamic f = GetPageInformation(page_username);
            about = f.about;
            //category = f.category;
           // location_street = f.location[0];
           // location_city = f.location[2];
            username = f.username;
            id = f.id;
            pagename = f.name;
            link = f.link;
            long likes = f.likes;
            dynamic p = getPosts(page_username);
            for (int i = 0; i < 25;i++ )
            {

                string message = p.posts[0][i][0].ToString();
                posts.Add(message);

            }
            int countPosts=posts.Count;
            MessageBox.Show("list of posts maaya" + countPosts);
              System.IO.File.AppendAllText("D://testf1", "RNN.NEWS info \n about: " + about + "\n id:" +id + " \n name" + pagename);
            System.IO.File.AppendAllText("D://testf1", "\n link:" +link);
        }


          public Form1()
        {
            InitializeComponent();


           // page_info();

        }

        /// <summary>
        /// ///////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

       


       
        ///
        private void button1_Click(object sender, EventArgs e)
        {
              int i = 0;
            string sql,sql3 = null;
            
            connetionString = @"Data Source=HALANSAARY-LP\SQLEXPRESS;Initial Catalog=facebook;Integrated Security=True";
            sql3 = "Select about from face_info";

            
         // sql = @"insert into pages_info(page_name,field_name,field_details,id) values(' "+pagename+" ','about','details',11)";
             //  sql = "insert into pages_info(page_name,field_name,field_details,id) values( 'candle','about','details is later soon',9)";
          //  sql = "insert into pages_info(page_name,field_name,field_details,id) values( @pagename,'about','details is later soon',11)";  
            sqlCnn = new SqlConnection(connetionString);

               
         
               //////////





               ////////////

               try
               {
                   sqlCnn.Open();

                   // Add the parameters for the InsertCommand.
                   //sqlCmd.Parameters.Add("@pagename", SqlDbType.NVarChar, 50, pagename);
              // sqlCmd.Parameters.AddWithValue("@pagename", pagename);
                   //sqlCmd.Parameters.Add("@details", SqlDbType.NVarChar, 50, about);

                   //SqlParameter param = new SqlParameter();
                   //param.ParameterName = "@pagename";
                   //param.Value = pagename;

                   //  sqlCmd.Parameters.Add(param);

                   // 3. add new parameter to command object


                   sqlCmd = new SqlCommand(sql3,sqlCnn);
                  // sqlCmd.Connection = sqlCnn;
                //   int xi = sqlCmd.ExecuteNonQuery();

                   //MessageBox.Show("weef hona");

                  // if (xi == 0)
                     //  MessageBox.Show("not added");
                   //else
                     //  MessageBox.Show("mabrooooooooooooook added");
                   /////////////////////     



                   //adapter.SelectCommand = sqlCmd;
                   //adapter.Fill(ds);
                   //for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                   //{
                   //    MessageBox.Show(ds.Tables[0].Rows[i].ItemArray[0] + " -- " + ds.Tables[0].Rows[i].ItemArray[1]);
                   //}
                   //adapter.Dispose();
                   SqlDataReader dr = sqlCmd.ExecuteReader();
                  
                   int j=0;

                   while (dr.Read())
                   {
                       String about_arr = (String)dr["about"];
                      // System.IO.File.WriteAllText("D://test4", about_arr, Encoding.UTF8);
                       about_lst.Add(about_arr);

                       //string[] readText = System.IO.File.ReadAllLines("D://test4");
                       //foreach (string s in readText)
                       //{
                       //    arrList.Add(s.Split());

                       //}

                       arrList.Add(about_arr.Split());
                   }     
                   MessageBox.Show("arr list length="+about_lst.ToArray().Length);

                   HashSet<string[]> hset = new HashSet<string[]>(arrList);
                   new_unique_lst = new List<String[]>(hset);
                   foreach (String[] unique in hset)
                   {
                       for (int k = 0; k < unique.Length; k++)

                       {
                           //System.IO.File.AppendAllText("D://unique.txt", Environment.NewLine);
                           //System.IO.File.AppendAllText("D://unique.txt", unique[k]);
                           insert(unique[k]);
                             }
                   }

                   MessageBox.Show("no error in insert all");
                   final_big_unique = distinct_lst();
                 
                   sqlCmd.Dispose();
                   sqlCnn.Close();
               }
               catch (Exception ex)
               {
                   MessageBox.Show("Can not open connection ! " + (e.ToString()));
               }
           }
        public static void insert(String unique)
        {
            String connString = @"Data Source=HALANSAARY-LP\SQLEXPRESS;Initial Catalog=facebook;Integrated Security=True";
            SqlConnection con = new SqlConnection(connString);
            try
            {
                con.Open();
                String sql = @"insert into about_words(words) values(' " + unique + " ' )";
                SqlCommand sqlCmd = new SqlCommand(sql, con);
                sqlCmd.ExecuteNonQuery();
                 con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("ErrorBlinkStyle in connection insert");
            }
        }


        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <returns></returns>
        /// 

        public static void delete_about()
        {
            String connString = @"Data Source=HALANSAARY-LP\SQLEXPRESS;Initial Catalog=facebook;Integrated Security=True";
            SqlConnection con = new SqlConnection(connString);
            try
            {
                con.Open();
                String sql = @"delete from about_words";
                SqlCommand sqlCmd = new SqlCommand(sql, con);
                sqlCmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("ErrorBlinkStyle in connection delete");
            }
        }



        ///////////////////
        public static List<String> distinct_lst() {

            List<String> lst = new List<string>();
            String connString = @"Data Source=HALANSAARY-LP\SQLEXPRESS;Initial Catalog=facebook;Integrated Security=True";
            SqlConnection con = new SqlConnection(connString);
            try
            {
                //int i = 0;
                con.Open();
                String sql = @"select distinct words from about_words";
                SqlCommand sqlCmd = new SqlCommand(sql, con);
                SqlDataReader dr= sqlCmd.ExecuteReader();
                while(dr.Read()){
                    String w = (String)dr["words"];
                    if (w.Trim().Length > 2)
                    {

                        string input = w.Trim();
                        string filtered = Regex.Replace(input, "[a-zA-Z0-9/.:()_#,\"’<->]", String.Empty);

                        if(!stop_wrd.Contains(filtered) && !stop_chars.Contains(filtered)){
                        lst.Add(filtered);

                        //     // add text to your concerned list.
                           System.IO.File.AppendAllText("D://unique1.txt", filtered+ ";", Encoding.UTF8);
                        }
                        
                        //  bool stringIsValid = Regex.IsMatch(inputString, @"^[a-zA-Z0-9\-]*?$");

                       // string cleanString = Regex.Replace(w.Trim(), @"[^a-zA-Z0-9\-]*?$", String.Empty);
                       //////////////////////

                       // string text = w.Trim();
                       // string reg = @"^((([\w]+\.[\w]+)+)|([\w]+))@(([\w]+\.)+)([A-Za-z]{1,3})$";


                       // if (!Regex.IsMatch(text, reg))
                       // {
                       //     lst.Add(w.Trim());

                       //     // add text to your concerned list.
                       //     System.IO.File.AppendAllText("D://unique1.txt", w.Trim() + ",", Encoding.UTF8);

                       // }
                       // else
                       // {
                       //    // String rep=text.Replace(reg,String.Empty);
                       //    String rep= Regex.Replace(w.Trim(), @"[^a-zA-Z0-9\-]", String.Empty);
                       //     lst.Add(rep.Trim());
                       //     System.IO.File.AppendAllText("D://unique1.txt", rep.Trim() + ",", Encoding.UTF8);

                       // }



                        /////////////
                       
                        
                      //  lst.ElementAt(i);
                        // System.IO.File.AppendAllText("D://unique.txt", Environment.NewLine);
                        //System.IO.File.AppendAllText("D://unique1.txt", w.Trim() + ",", Encoding.UTF8);

                    }
                  //  i++;
                }
                System.IO.File.AppendAllText("D://unique1.txt", Environment.NewLine);

                con.Close();

                delete_about();
               // System.IO.File.AppendAllText("D://unique1.txt", lst.Reverse());

            }
            catch (Exception ex)
            {

                MessageBox.Show("ErrorBlinkStyle in connection");
            }
            return lst;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // page_info("RNN.NEWS");
        }


         //foreach(String s in unique_items){
         //        bool elmnt=  lst.Contains(s);
         //        if (elmnt)

         //            System.IO.File.AppendAllText("D://distinct.txt", "  1");
         //        else
         //            System.IO.File.AppendAllText("D://distinct.txt","  0");
               
         //      }


        //private static bool AllElementInList(List<string[]> list, List<string> arr)
        //{
        //    return list.Select(ar2 => arr.All(ar2.Contains)).FirstOrDefault();
        //}



        
       

        private void button2_Click(object sender, EventArgs e)
        {
            //bool found = false;


            //bool flag = AllElementInList(new_unique_lst, final_big_unique);
              

            foreach (String[] arr in new_unique_lst)
            {

                System.IO.File.AppendAllText("D://unique1.txt", Environment.NewLine, Encoding.UTF8);

                
                foreach (String s in final_big_unique)
                {

                    //    System.IO.File.AppendAllText("D://unique.txt", Environment.NewLine);

                    //foreach (String f in arr) { 
                    //if (arr.Any(s.Contains)) {
                    //    found = true;
                    //    break;
                    //}
                   // List<String> lst = new List<string>(arr);

                    if (arr.Contains(s))
                    {
                        //found = true;
                        //break;

                        System.IO.File.AppendAllText("D://unique1.txt", "1;", Encoding.UTF8);

                    }
                    else
                        System.IO.File.AppendAllText("D://unique1.txt", "0;", Encoding.UTF8);


                    //if (found)
                    //    System.IO.File.AppendAllText("D://unique.txt", "1");

                    //else if(!found)
                    //    System.IO.File.AppendAllText("D://unique.txt", "0");

                }
                       
            
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String page_username = txt_page_username.Text;
            page_info(page_username);
            // about, id, username, pagename,link

            String connString = @"Data Source=HALANSAARY-LP\SQLEXPRESS;Initial Catalog=facebook;Integrated Security=True";
            SqlConnection con = new SqlConnection(connString);
            try
            {
                con.Open();
                String sql = @"insert into face_info (about,page_id,page_username,page_name,page_link) values( ' " + about + " ' , ' " + id.ToString() + " ' , ' " + username + " ' , ' " + pagename + " ' , ' " + link + " ' )";
                SqlCommand sqlCmd = new SqlCommand(sql, con);
               int rows= sqlCmd.ExecuteNonQuery();
               if (rows > 0)
                   MessageBox.Show("inserted rows :"+rows);
                else
                   MessageBox.Show(" no inserted rows :" + rows);
                con.Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show("ErrorBlinkStyle in connection insert page");
            }
        }


       
    }
}
