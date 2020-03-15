using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace QuanLyNhanVien
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            show();
        }

        public void show()
        {
            XmlHandler handler = new XmlHandler(); //Tao moi doi tuong XmlHandler
            XmlDocument doc = new XmlDocument(); //Tao moi doi tuong XmlDocument
            string filename = @"C:\Users\ADMIN\Desktop\Train C#\QuanLyNhanVien\QuanLyNhanVien\QuanLyNhanVien\congty.xml";
            //Duong dan toi file
            List<Employee> empList = new List<Employee>();
            //Tao moi 1 danh sach luu tru cac nhan vien
            handler.loadDataFromDoc(doc, filename, empList);
            //lay du lieu cac nhan vien tu file xml them vao danh sach nhan vien
            dgvInfor.Rows.Clear(); //xoa het du lieu cac dong
            int rowIndex = 0; //chi so cua dong cua bang
            foreach (Employee emp in empList)
            {
                dgvInfor.Rows.Add(); //Them mot dong moi
                //chen du lieu vao bang
                dgvInfor.Rows[rowIndex].Cells[0].Value = emp.EmpID;
                dgvInfor.Rows[rowIndex].Cells[1].Value = emp.EmpName;
                dgvInfor.Rows[rowIndex].Cells[2].Value = emp.Salary;
                dgvInfor.Rows[rowIndex].Cells[3].Value = emp.DeptName;
                dgvInfor.Rows[rowIndex].Cells[4].Value = emp.DeptTel;

                rowIndex++;//Tang chi so cua cot len
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            XmlHandler handler = new XmlHandler();
            //Khai bao cac bien luu tru thong tin nhan vien do nguoi dung nhap
            string empId, empName, deptName, deptTel;
            float salary;
            try
            {
                //Lay ra cac gia tri
                empId = txtEmpId.Text;
                empName = txtEmpName.Text;
                salary = float.Parse(txtSalary.Text);
                deptName = txtDeptName.Text;
                deptTel = txtDeptTel.Text;
                //Tao moi nhan vien co cac thong tin o tren
                if (!notEmptyFields()) //neu chua nhap day du thong tin
                    MessageBox.Show("Any fields must not be empty", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Employee emp = new Employee(empId, empName, salary, deptName, deptTel);
                    if (!handler.add(emp)) //Neu them khong thanh cong
                        MessageBox.Show("Can't not add duplicate Emp. ID: " + empId, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else //Thanh cong
                    {
                        MessageBox.Show("Sucessfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        show(); //load lai table
                        clearTextBox(); //xoa noi dung tren text box
                        txtEmpId.Focus(); //set focus vao o text box EmpId
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool notEmptyFields()
        {
            if (txtEmpId.Text.Trim() == "" || txtEmpName.Text.Trim() == "")
                return false;
            if (txtSalary.Text.Trim() == "" || txtDeptName.Text.Trim() == "")
                return false;
            if (lblDeptTel.Text.Trim() == "")
                return false;
            return true;
        }

        public void clearTextBox()
        {
            txtEmpId.Text = "";
            txtEmpName.Text = "";
            txtSalary.Text = "";
            txtDeptName.Text = "";
            txtDeptTel.Text = "";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            XmlHandler handler = new XmlHandler();
            //Khai bao cac bien luo tru thong tin nhan vien do nguoi dung nhap
            string empId, empName, deptName, deptTel;
            float salary;
            try
            {
                //Lay ra cac gia tri
                empId = txtEmpId.Text;
                empName = txtEmpName.Text;
                salary = float.Parse(txtSalary.Text);
                deptName = txtDeptName.Text;
                deptTel = txtDeptTel.Text;
                //Tao moi nhan vien co cac thong tin o tren
                if (!notEmptyFields()) //neu chua nhap day du thong tin
                    MessageBox.Show("Any fields must not be empty", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    Employee emp = new Employee(empId, empName, salary, deptName, deptTel);
                    if (!handler.edit(emp)) //Neu sua khong thanh cong
                        MessageBox.Show("Emp. ID: " + empId + " is not exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else //Thanh cong
                    {
                        MessageBox.Show("Sucessfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        show(); //load lai table
                        clearTextBox(); //xoa noi dung tren text box
                        txtEmpId.Focus(); //set focus vao o text box EmpId
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void onCellClicked(object sender, DataGridViewCellEventArgs e)
        {
            int rowSeleted = dgvInfor.CurrentCell.RowIndex; //Lay ra dong hien tai dang duoc chon
            //hien thi cac thong tin o dong hien tai len o text box tuong ung
            txtEmpId.Text = (string)dgvInfor.Rows[rowSeleted].Cells[0].Value;
            txtEmpName.Text = (string)dgvInfor.Rows[rowSeleted].Cells[1].Value;
            float salary = (float)dgvInfor.Rows[rowSeleted].Cells[2].Value;
            txtSalary.Text = salary.ToString();
            txtDeptName.Text = (string)dgvInfor.Rows[rowSeleted].Cells[3].Value;
            txtDeptTel.Text = (string)dgvInfor.Rows[rowSeleted].Cells[4].Value;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string empId = txtEmpId.Text;
            if (empId.Trim() == "")
                MessageBox.Show("EmpId must not be empty", "Waring", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                XmlHandler handler = new XmlHandler();
                if (!handler.delete(empId)) //Neu xoa khong thanh cong
                    MessageBox.Show("Emp. ID: " + empId + " is not exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else //Thanh cong
                {
                    MessageBox.Show("Sucessfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    show(); //load lai table
                    clearTextBox(); //xoa noi dung tren text box
                    txtEmpId.Focus(); //set focus vao o text box EmpId
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
