using System;
using System.Data.Odbc;
using System.Diagnostics;
using System.Windows.Forms;

namespace cs_form_framework_mysql_03
{
    public partial class Form1 : Form
    {
        private int kaiwa = 1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OdbcConnection myCon = CreateConnection();

            // MySQL の処理

            string target = this.scode.Text;

            // SQL
            string myQuery =
                $@"SELECT
                    社員マスタ.*,
                    DATE_FORMAT(生年月日,'%Y-%m-%d') as 誕生日
                    from 社員マスタ
                    where 社員コード = '{target}'";

            // SQL実行用のオブジェクトを作成
            OdbcCommand myCommand = new OdbcCommand();

            // 実行用オブジェクトに必要な情報を与える
            myCommand.CommandText = myQuery;    // SQL
            myCommand.Connection = myCon;       // 接続

            // 次でする、データベースの値をもらう為のオブジェクトの変数の定義
            OdbcDataReader myReader;

            // SELECT を実行した結果を取得
            myReader = myCommand.ExecuteReader();

            // myReader からデータが読みだされる間ずっとループ
            if (myReader.Read())
            {
                // 列名より列番号を取得
                int index = myReader.GetOrdinal("氏名");
                // 列番号で、値を取得して文字列化
                string text = myReader.GetValue(index).ToString();
                // 出力ウインドウに出力
                Debug.WriteLine($"Debug:{text}");

                this.sname.Text = text;
                this.button1.Text = "送信";

            }

            myReader.Close();

            myCon.Close();

            // 第二会話に移行
            kaiwa = 2;
            Form1_Load(null, null);

        }

        private OdbcConnection CreateConnection()
        {
            // 接続文字列の作成
            OdbcConnectionStringBuilder builder = new OdbcConnectionStringBuilder();
            builder.Driver = "MySQL ODBC 8.0 Unicode Driver";
            // 接続用のパラメータを追加
            builder.Add("server", "localhost");
            builder.Add("database", "lightbox");
            builder.Add("uid", "root");
            builder.Add("pwd", "");

            string work = builder.ConnectionString;

            Console.WriteLine(builder.ConnectionString);

            // 接続の作成
            OdbcConnection myCon = new OdbcConnection();

            // MySQL の接続準備完了
            myCon.ConnectionString = builder.ConnectionString;

            // MySQL に接続
            myCon.Open();

            return myCon;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("更新しますか?", "確認", MessageBoxButtons.OKCancel);
            if (result != DialogResult.OK)
            {
                return;
            }

            // 更新処理
            OdbcConnection myCon = CreateConnection();

            // MySQL の処理

            string target = this.scode.Text;
            string sname = this.sname.Text;

            // SQL
            string myQuery =
                $@"UPDATE 社員マスタ
                    SET 氏名 = '{sname}'
                    where 社員コード = '{target}'";

            // SQL実行用のオブジェクトを作成
            OdbcCommand myCommand = new OdbcCommand();

            // 実行用オブジェクトに必要な情報を与える
            myCommand.CommandText = myQuery;    // SQL
            myCommand.Connection = myCon;       // 接続

            myCommand.ExecuteNonQuery();

            myCon.Close();

            clearForm();

        }

        private void clearForm()
        {
            scode.Clear();
            sname.Clear();

            // 第一会話に移行
            kaiwa = 1;
            Form1_Load(null, null);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 初期状態
            if ( kaiwa == 1 )
            {
                scode.Enabled = true;
                button1.Enabled = true;

                sname.Enabled = false;
                button2.Enabled = false;
                cancel.Enabled = false;
            }
            // 第二会話
            if (kaiwa == 2)
            {
                scode.Enabled = false;
                button1.Enabled = false;

                sname.Enabled = true;
                button2.Enabled = true;
                cancel.Enabled = true;
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {

            clearForm();

        }
    }
}
