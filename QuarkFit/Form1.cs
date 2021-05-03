using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;

namespace QuarkFit
{
    public partial class QuarkFit : Form
    {

        public QuarkFit()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\QuarkFitDB.mdf;Integrated Security=True");
        public int userID;

        private void QuarkFit_Load(object sender, EventArgs e)
        {
            GetUsersRecord();
            fillSavedTrains();
            usersListLayout();
            GetBirthUsers();
            birthsListLayout();
            this.WindowState = FormWindowState.Maximized;

            usersList.ScrollBars = ScrollBars.Vertical;
            usersList.EditMode = DataGridViewEditMode.EditProgrammatically;

            birthsList.ScrollBars = ScrollBars.Vertical;
            birthsList.EditMode = DataGridViewEditMode.EditProgrammatically;

            btnSave.Visible = true;
            trainEdit.Visible = false;

            
        }

        private void usersListLayout()
        {
            usersList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            usersList.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;

            usersList.Columns[1].HeaderText = "Nome do Aluno";
            usersList.Columns[5].HeaderText = "Data Limite";

            usersList.RowHeadersVisible = false;
            usersList.Columns[0].Visible = false;
            usersList.Columns[2].Visible = false;
            usersList.Columns[3].Visible = false;
            usersList.Columns[4].Visible = false;
            usersList.Columns[6].Visible = false;
            usersList.Columns[7].Visible = false;
            usersList.Columns[8].Visible = false;
            usersList.Columns[9].Visible = false;

            usersList.Columns[1].Width = 319;
            usersList.Columns[5].Width = 119;
        }

        private void birthsListLayout()
        {
            birthsList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            birthsList.Columns[9].SortMode = DataGridViewColumnSortMode.NotSortable;

            birthsList.Columns[1].HeaderText = "Aniversariante";
            birthsList.Columns[9].HeaderText = "Data";

            birthsList.RowHeadersVisible = false;
            birthsList.Columns[0].Visible = false;
            birthsList.Columns[2].Visible = false;
            birthsList.Columns[3].Visible = false;
            birthsList.Columns[4].Visible = false;
            birthsList.Columns[5].Visible = false;
            birthsList.Columns[6].Visible = false;
            birthsList.Columns[7].Visible = false;
            birthsList.Columns[8].Visible = false;

            birthsList.Columns[1].Width = 145;
            birthsList.Columns[9].Width = 107;
        }

        private void GetUsersRecord()
        {
            SqlCommand cmd = new SqlCommand("SELECT * from users", con);
            DataTable data = new DataTable();

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            data.Load(reader);

            con.Close();

            usersList.DataSource = data;

        }

        private void GetBirthUsers()
        {
            DateTime today = DateTime.Today;

            int todayMonth = today.Month;

            SqlCommand cmd;

            if (todayMonth < 10)
            {
                con.Open();
                cmd = new SqlCommand("SELECT * FROM users WHERE userBirth LIKE '____" + todayMonth + "%'", con);
            }
            else
            {
                con.Open();
                cmd = new SqlCommand("SELECT * FROM users WHERE userBirth LIKE '___" + todayMonth + "%'", con);
            }

            cmd.ExecuteNonQuery();
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(data);

            con.Close();

            birthsList.DataSource = data;

        }

        private void GetTrainsRecord()
        {
            SqlCommand cmd = new SqlCommand("SELECT * from trains", con);
            DataTable trainsData = new DataTable();

            con.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            savedTrains.Items.Clear();

            while (reader.Read())
            {
                savedTrains.Items.Add(reader[1]);
            }
            reader.Close();
            reader.Dispose();

            con.Close();

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            if (userName.Text != "")
            {
                if (userLimit.Text != "  /  /")
                {
                    if (userBirth.Text != "  /  /")
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO users VALUES (@userName, @userCpf, @userGoal, @userPlan, @userLimit, @userTime, @userContact, @userObservations, @userBirth)", con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@userName", userName.Text);
                        cmd.Parameters.AddWithValue("@userCpf", userCpf.Text);
                        cmd.Parameters.AddWithValue("@userGoal", userGoal.Text);
                        cmd.Parameters.AddWithValue("@userPlan", userPlan.Text);
                        cmd.Parameters.AddWithValue("@userLimit", userLimit.Text);
                        cmd.Parameters.AddWithValue("@userTime", userTime.Text);
                        cmd.Parameters.AddWithValue("@userContact", userContact.Text);
                        cmd.Parameters.AddWithValue("@userObservations", userObservations.Text);
                        cmd.Parameters.AddWithValue("@userBirth", userBirth.Text);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Novo aluno adicionado com sucesso!", "Salvo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        GetUsersRecord();
                        GetBirthUsers();

                        userName.Clear();
                        userCpf.Clear();
                        userGoal.Text = "";
                        userPlan.Text = "";
                        userLimit.Clear();
                        userBirth.Clear();
                        userTime.Clear();
                        userContact.Clear();
                        userObservations.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Data de Nascimento em branco...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Data Limite em branco...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Nome do aluno em branco...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void birthsList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            userName.Text = birthsList.SelectedRows[0].Cells[1].Value.ToString();
            userCpf.Text = birthsList.SelectedRows[0].Cells[2].Value.ToString();
            userGoal.Text = birthsList.SelectedRows[0].Cells[3].Value.ToString();
            userPlan.Text = birthsList.SelectedRows[0].Cells[4].Value.ToString();
            userLimit.Text = birthsList.SelectedRows[0].Cells[5].Value.ToString();
            userTime.Text = birthsList.SelectedRows[0].Cells[6].Value.ToString();
            userContact.Text = birthsList.SelectedRows[0].Cells[7].Value.ToString();
            userObservations.Text = birthsList.SelectedRows[0].Cells[8].Value.ToString();
            userBirth.Text = birthsList.SelectedRows[0].Cells[9].Value.ToString();
        }

        private void usersList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            userID = Convert.ToInt32(usersList.SelectedRows[0].Cells[0].Value);
            userName.Text = usersList.SelectedRows[0].Cells[1].Value.ToString();
            userCpf.Text = usersList.SelectedRows[0].Cells[2].Value.ToString();
            userGoal.Text = usersList.SelectedRows[0].Cells[3].Value.ToString();
            userPlan.Text = usersList.SelectedRows[0].Cells[4].Value.ToString();
            userLimit.Text = usersList.SelectedRows[0].Cells[5].Value.ToString();
            userTime.Text = usersList.SelectedRows[0].Cells[6].Value.ToString();
            userContact.Text = usersList.SelectedRows[0].Cells[7].Value.ToString();
            userObservations.Text = usersList.SelectedRows[0].Cells[8].Value.ToString();
            userBirth.Text = usersList.SelectedRows[0].Cells[9].Value.ToString();

        }

        private void btnEdit_Click_1(object sender, EventArgs e)
        {
            if (userID > 0)
            {
                if (MessageBox.Show("Deseja editar as informações do aluno abaixo? \r\n" + "➥ " + userName.Text, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("UPDATE users SET userName = @userName, userCpf = @userCpf, userGoal = @userGoal, userPlan = @userPlan, userLimit = @userLimit, userTime = @userTime,userContact = @userContact, userObservations = @userObservations, userBirth = @userBirth WHERE userID = @userID", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@userName", userName.Text);
                    cmd.Parameters.AddWithValue("@userCpf", userCpf.Text);
                    cmd.Parameters.AddWithValue("@userGoal", userGoal.Text);
                    cmd.Parameters.AddWithValue("@userPlan", userPlan.Text);
                    cmd.Parameters.AddWithValue("@userLimit", userLimit.Text);
                    cmd.Parameters.AddWithValue("@userTime", userTime.Text);
                    cmd.Parameters.AddWithValue("@userContact", userContact.Text);
                    cmd.Parameters.AddWithValue("@userObservations", userObservations.Text);
                    cmd.Parameters.AddWithValue("@userBirth", userBirth.Text);
                    cmd.Parameters.AddWithValue("@userID", this.userID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    GetUsersRecord();
                    GetBirthUsers();
                }
            }
            else
            {
                MessageBox.Show("Selecione um aluno para editar...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (userID > 0)
            {
                if (MessageBox.Show("Deseja excluir as informações do aluno abaixo? \r\n" + "➥ " + userName.Text, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE userID = @userID", con);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@userID", this.userID);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Aluno excluído com sucesso!", "Excluído", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    GetUsersRecord();
                    GetBirthUsers();

                    userID = 0;
                    userName.Clear();
                    userCpf.Clear();
                    userGoal.Text = "";
                    userPlan.Text = "";
                    userLimit.Clear();
                    userBirth.Clear();
                    userTime.Clear();
                    userContact.Clear();
                    userObservations.Clear();
                }
            }
            else
            {
                MessageBox.Show("Selecione um aluno para excluir...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void searchbar_TextChanged(object sender, EventArgs e)
        {
            string barContent = searchbar.Text;

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM users WHERE userName LIKE '%" + barContent + "%' ", con);
            DataTable data = new DataTable();

            adapter.Fill(data);
            usersList.DataSource = data;
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (trainOk())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO trains VALUES (@trainName, @trainExercise1, @trainExercise2, @trainExercise3, @trainExercise4, @trainExercise5, @trainExercise6, @trainExercise7, @trainExercise8, @trainDate)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@trainName", trainName.Text);
                cmd.Parameters.AddWithValue("@trainExercise1", trainExercise1.Text);
                cmd.Parameters.AddWithValue("@trainExercise2", trainExercise2.Text);
                cmd.Parameters.AddWithValue("@trainExercise3", trainExercise3.Text);
                cmd.Parameters.AddWithValue("@trainExercise4", trainExercise4.Text);
                cmd.Parameters.AddWithValue("@trainExercise5", trainExercise5.Text);
                cmd.Parameters.AddWithValue("@trainExercise6", trainExercise6.Text);
                cmd.Parameters.AddWithValue("@trainExercise7", trainExercise7.Text);
                cmd.Parameters.AddWithValue("@trainExercise8", trainExercise8.Text);
                cmd.Parameters.AddWithValue("@trainDate", trainDate.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Novo treino salvo com sucesso!", "Salvo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetTrainsRecord();
            }
            else
            {
                MessageBox.Show("Nome do treino em branco...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool trainOk()
        {
            if (trainName.Text == String.Empty)
            {
                return false;
            }

            return true;
        }

        void fillSavedTrains()
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT trainName FROM trains", con);
            cmd.ExecuteNonQuery();
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            adapter.Fill(data);
            foreach (DataRow row in data.Rows)
            {
                savedTrains.Items.Add(row["trainName"].ToString());
            }

            con.Close();
        }

        private void savedTrains_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cbContent = savedTrains.Text;
            DataTable data = new DataTable();

            SqlCommand cmd = new SqlCommand("SELECT trainExercise1, trainExercise2, trainExercise3, trainExercise4, trainExercise5, trainExercise6, trainExercise7, trainExercise8, trainDate FROM trains WHERE trainName LIKE '%" + cbContent + "%' ", con);

            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            data.Load(reader);

            foreach (DataRow row in data.Rows)
            {
                trainName.Text = savedTrains.Text;
                trainExercise1.Text = row["trainExercise1"].ToString();
                trainExercise2.Text = row["trainExercise2"].ToString();
                trainExercise3.Text = row["trainExercise3"].ToString();
                trainExercise4.Text = row["trainExercise4"].ToString();
                trainExercise5.Text = row["trainExercise5"].ToString();
                trainExercise6.Text = row["trainExercise6"].ToString();
                trainExercise7.Text = row["trainExercise7"].ToString();
                trainExercise8.Text = row["trainExercise8"].ToString();
                trainDate.Text = row["trainDate"].ToString();
            }

            btnSave.Visible = false;
            trainEdit.Visible = true;
            con.Close();
        }

        private void btnInspect_Click_1(object sender, EventArgs e)
        {

            DateTime today = DateTime.Today;
            string usersMessage = "Os planos dos seguintes alunos já se encerraram:\r\n";
            string trainsMessage = "Os treinos abaixo estão fora da data limite:\r\n";

            int todayDay = today.Day;
            int todayMonth = today.Month;
            int todayYear = today.Year;

            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT userName, userLimit FROM users", con);
            cmd.ExecuteNonQuery();
            DataTable data = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            string[] delayedUsers = new String[1];

            adapter.Fill(data);
            foreach (DataRow row in data.Rows)
            {
                string userDate = row["userLimit"].ToString();

                int day = Convert.ToInt32(userDate[0].ToString() + userDate[1].ToString());
                int month = Convert.ToInt32(userDate[3].ToString() + userDate[4].ToString());
                int year = Convert.ToInt32(userDate[6].ToString() + userDate[7].ToString() + userDate[8].ToString() + userDate[9].ToString());

                if (year < todayYear)
                {
                    delayedUsers[delayedUsers.Length - 1] = (row["userName"].ToString());
                    Array.Resize(ref delayedUsers, delayedUsers.Length + 1);
                }
                else if (year > todayYear) { }
                else if (year == todayYear)
                {
                    if (month < todayMonth)
                    {
                        delayedUsers[delayedUsers.Length - 1] = row["userName"].ToString();
                        Array.Resize(ref delayedUsers, delayedUsers.Length + 1);
                    }
                    else if (month > todayMonth) { }
                    else if (month == todayMonth)
                    {
                        if (day < todayDay)
                        {
                            delayedUsers[delayedUsers.Length - 1] = row["userName"].ToString();
                            Array.Resize(ref delayedUsers, delayedUsers.Length + 1);
                        }
                        else if (day > todayDay) { }
                        else if (day == todayDay) { }
                    }
                }
            }

            con.Close();

            con.Open();
            SqlCommand cmdTrain = new SqlCommand("SELECT trainName, trainDate FROM trains", con);
            cmdTrain.ExecuteNonQuery();
            DataTable dataTrain = new DataTable();
            SqlDataAdapter adapterTrain = new SqlDataAdapter(cmdTrain);

            string[] delayedTrains = new String[1];

            adapterTrain.Fill(dataTrain);
            foreach (DataRow row in dataTrain.Rows)
            {
                string trainDate = row["trainDate"].ToString();

                int dayTrain = Convert.ToInt32(trainDate[0].ToString() + trainDate[1].ToString());
                int monthTrain = Convert.ToInt32(trainDate[3].ToString() + trainDate[4].ToString());
                int yearTrain = Convert.ToInt32(trainDate[6].ToString() + trainDate[7].ToString() + trainDate[8].ToString() + trainDate[9].ToString());

                if (yearTrain < todayYear)
                {
                    delayedTrains[delayedTrains.Length - 1] = (row["trainName"].ToString());
                    Array.Resize(ref delayedTrains, delayedTrains.Length + 1);
                }
                else if (yearTrain > todayYear) { }
                else if (yearTrain == todayYear)
                {
                    if (monthTrain < todayMonth)
                    {
                        delayedTrains[delayedTrains.Length - 1] = row["trainName"].ToString();
                        Array.Resize(ref delayedTrains, delayedTrains.Length + 1);
                    }
                    else if (monthTrain > todayMonth) { }
                    else if (monthTrain == todayMonth)
                    {
                        if (dayTrain < todayDay)
                        {
                            delayedTrains[delayedTrains.Length - 1] = row["trainName"].ToString();
                            Array.Resize(ref delayedTrains, delayedTrains.Length + 1);
                        }
                        else if (dayTrain > todayDay) { }
                        else if (dayTrain == todayDay) { }
                    }
                }
            }

            con.Close();

            if (delayedUsers.Length > 1)
            {
                for (int i = 0; i < delayedUsers.Length; i++)
                {
                    if (i != delayedUsers.Length - 1)
                    {
                        usersMessage += "➥ " + delayedUsers[i] + "\r\n";
                    }
                }

                MessageBox.Show(usersMessage, "Alunos Atrasados!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Todos os alunos estão com seus planos ativos!", "Alunos OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            if (delayedTrains.Length > 1)
            {
                for (int i = 0; i < delayedTrains.Length; i++)
                {
                    if (i != delayedTrains.Length - 1)
                    {
                        trainsMessage += "➥ " + delayedTrains[i] + "\r\n";
                    }
                }

                MessageBox.Show(trainsMessage, "Treinos Expirados!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Todos os treinos estão dentro da data limite!", "Treinos OK!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



        }

        private void btnSend_Click_1(object sender, EventArgs e)
        {

            if (userContact.Text != "")
            {
                System.Diagnostics.Process pStart = new System.Diagnostics.Process();
                string message = "";

                if (trainName.Text != "")
                {
                    message = "*" + trainName.Text + "*\r\n\r\n";
                }
                string number = "+55" + userContact.Text;

                if (trainExercise1.Text != "")
                {
                    message += "➥ " + trainExercise1.Text + "\r\n";
                }
                if (trainExercise2.Text != "")
                {
                    message += "➥ " + trainExercise2.Text + "\r\n";
                }
                if (trainExercise3.Text != "")
                {
                    message += "➥ " + trainExercise3.Text + "\r\n";
                }
                if (trainExercise4.Text != "")
                {
                    message += "➥ " + trainExercise4.Text + "\r\n";
                }
                if (trainExercise5.Text != "")
                {
                    message += "➥ " + trainExercise5.Text + "\r\n";
                }
                if (trainExercise6.Text != "")
                {
                    message += "➥ " + trainExercise6.Text + "\r\n";
                }
                if (trainExercise7.Text != "")
                {
                    message += "➥ " + trainExercise7.Text + "\r\n";
                }
                if (trainExercise8.Text != "")
                {
                    message += "➥ " + trainExercise8.Text + "\r\n";
                }

                if (trainDate.Text != "  /  /")
                {
                    message += "\r\n*Obs.:* Atenção, esse treino tem uma data de término prevista para " + trainDate.Text + ". Quando chegar esse dia, procure seu professor!\r\n";
                }

                message = Uri.EscapeDataString(message);

                string url = "https://wa.me/" + number + "?text=" + message;

                pStart.StartInfo = new System.Diagnostics.ProcessStartInfo(url);
                pStart.Start();
            }
            else
            {
                MessageBox.Show("Número do aluno não foi encontrado...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnNew_Click_1(object sender, EventArgs e)
        {
            userName.Clear();
            userCpf.Clear();
            userGoal.Text = "";
            userPlan.Text = "";
            userLimit.Clear();
            userBirth.Clear();
            userTime.Clear();
            userContact.Clear();
            userObservations.Clear();
        }

        private void btnDeleteTrain_Click_1(object sender, EventArgs e)
        {
            if (trainOk())
            {
                if (MessageBox.Show("Deseja excluir as informações do treino abaixo? \r\n" + "➥ " + trainName.Text, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM trains WHERE trainName = @trainName", con);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.AddWithValue("@trainName", trainName.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Treino apagado com sucesso!", "Apagado!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    GetTrainsRecord();

                    savedTrains.Text = "";
                    trainExercise1.Clear();
                    trainExercise2.Clear();
                    trainExercise3.Clear();
                    trainExercise4.Clear();
                    trainExercise5.Clear();
                    trainExercise6.Clear();
                    trainExercise7.Clear();
                    trainExercise8.Clear();
                    trainName.Clear();
                    trainDate.Clear();
                }
            }
            else
            {
                MessageBox.Show("Selecione um treino para apagar...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void info_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process pStart = new System.Diagnostics.Process();
            pStart.StartInfo = new System.Diagnostics.ProcessStartInfo("https://youtu.be/NToF1HoW6yA");
            pStart.Start();
        }

        public void trainName_TextChanged(object sender, EventArgs e)
        {
            string trainNameContent = trainName.Text;

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM trains WHERE trainName LIKE '%" + trainNameContent + "%' ", con);
            DataTable data = new DataTable();
            int flag = 0;

            adapter.Fill(data);
            foreach (DataRow row in data.Rows)
            {
                if (trainNameContent == row["trainName"].ToString())
                {
                    flag = 1;
                }
            }

            if (flag == 1)
            {
                btnSave.Visible = false;
                trainEdit.Visible = true;
            }
            else
            {
                btnSave.Visible = true;
                trainEdit.Visible = false;
            }

            if (trainName.Text == "")
            {
                btnSend.ImageLocation = @"C:\Users\aipom\Downloads\contact.png";
            }
            else
            {
                btnSend.ImageLocation = @"C:\Users\aipom\Downloads\sendtrain.png";
            }
        }

        private void trainEdit_Click(object sender, EventArgs e)
        {
            string trainNameContent = trainName.Text;

            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM trains WHERE trainName LIKE '%" + trainNameContent + "%' ", con);
            DataTable data = new DataTable();

            adapter.Fill(data);
            foreach (DataRow row in data.Rows)
            {
                if (trainNameContent == row["trainName"].ToString())
                {
                    int trainID = Convert.ToInt32(row["trainID"].ToString());

                    if (MessageBox.Show("Deseja editar as informações do treino acima? \r\n" + "➥ " + trainName.Text, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("UPDATE trains SET trainDate = @trainDate, trainExercise1 = @trainExercise1, trainExercise2 = @trainExercise2, trainExercise3 = @trainExercise3, trainExercise4 = @trainExercise4, trainExercise5 = @trainExercise5, trainExercise6 = @trainExercise6, trainExercise7 = @trainExercise7, trainExercise8 = @trainExercise8 WHERE trainID = @trainID", con);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@trainDate", trainDate.Text);
                        cmd.Parameters.AddWithValue("@trainExercise1", trainExercise1.Text);
                        cmd.Parameters.AddWithValue("@trainExercise2", trainExercise2.Text);
                        cmd.Parameters.AddWithValue("@trainExercise3", trainExercise3.Text);
                        cmd.Parameters.AddWithValue("@trainExercise4", trainExercise4.Text);
                        cmd.Parameters.AddWithValue("@trainExercise5", trainExercise5.Text);
                        cmd.Parameters.AddWithValue("@trainExercise6", trainExercise6.Text);
                        cmd.Parameters.AddWithValue("@trainExercise7", trainExercise7.Text);
                        cmd.Parameters.AddWithValue("@trainExercise8", trainExercise8.Text);
                        cmd.Parameters.AddWithValue("@trainID", trainID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        GetTrainsRecord();
                    }
                }
            }     
        }

        private void QuarkFit_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Maximized)
            {
                usersList.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                usersList.Columns[4].SortMode = DataGridViewColumnSortMode.NotSortable;
                usersList.Columns[5].SortMode = DataGridViewColumnSortMode.NotSortable;
                usersList.Columns[6].SortMode = DataGridViewColumnSortMode.NotSortable;
                usersList.Columns[7].SortMode = DataGridViewColumnSortMode.NotSortable;

                usersList.Columns[1].HeaderText = "Nome do Aluno";
                usersList.Columns[4].HeaderText = "Plano";
                usersList.Columns[5].HeaderText = "Data Limite";
                usersList.Columns[6].HeaderText = "Horário";
                usersList.Columns[7].HeaderText = "Contato";

                usersList.RowHeadersVisible = false;
                usersList.Columns[0].Visible = false;
                usersList.Columns[2].Visible = false;
                usersList.Columns[3].Visible = false;
                usersList.Columns[4].Visible = true;
                usersList.Columns[6].Visible = true;
                usersList.Columns[7].Visible = true;
                usersList.Columns[8].Visible = false;
                usersList.Columns[9].Visible = false;

                usersList.Columns[1].Width = Convert.ToInt32(this.Width * 0.70 * 0.37);
                usersList.Columns[4].Width = Convert.ToInt32(this.Width * 0.20 * 0.37);
                usersList.Columns[5].Width = Convert.ToInt32(this.Width * 0.20 * 0.37);
                usersList.Columns[6].Width = Convert.ToInt32(this.Width * 0.37 * 0.37);
                usersList.Columns[7].Width = Convert.ToInt32(this.Width * 0.24 * 0.37);

                usersList.MaximumSize = new Size(Convert.ToInt32(this.Width * 0.70 * 0.37 + this.Width * 0.20 * 0.37 + this.Width * 0.20 * 0.37 + this.Width * 0.37 * 0.37 + this.Width * 0.24 * 0.37), 780);
                birthsList.MaximumSize = new Size(252, 230);

                /*trainExercise8.Margin = new Padding(30, 0, 0, 0);
                trainExercise8.Margin = new Padding(0, 0, 0, 30);*/
            }
        }

        private void usersList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (userID > 0)
                {
                    if (MessageBox.Show("Deseja excluir as informações do aluno abaixo? \r\n" + "➥ " + userName.Text, "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand("DELETE FROM users WHERE userID = @userID", con);
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.AddWithValue("@userID", this.userID);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        MessageBox.Show("Aluno excluído com sucesso!", "Excluído", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        GetUsersRecord();
                        GetBirthUsers();

                        userID = 0;
                        userName.Clear();
                        userCpf.Clear();
                        userGoal.Text = "";
                        userPlan.Text = "";
                        userLimit.Clear();
                        userBirth.Clear();
                        userTime.Clear();
                        userContact.Clear();
                        userObservations.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Selecione um aluno para excluir...", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void birthsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                e.SuppressKeyPress = true;
            }

            if (e.KeyCode == Keys.Right)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}
