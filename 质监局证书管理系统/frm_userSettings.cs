﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Metro;
using DevComponents.AdvTree;
using System.Configuration;
using Quality;
using Quality.Model;
using Quality.BLL;
namespace 质监局证书管理系统
{
    public partial class frm_userSettings : DevComponents.DotNetBar.Metro.MetroForm
    {
        private UserBLL bll;
        private RoleBLL roleBll;
        private bool isEditing;
        public frm_userSettings()
        {
            InitializeComponent();
            InitRealnameList(tb_autoRealname.Text.Trim());
            InitRoleDropdownList();
            //this.DisableForm();
            
        }
        public void InitRealnameList(string realname)
        {
            bll=new UserBLL();
            adv_realnameList.Nodes.Clear();
            IList<Users> users = bll.GetRealNameList(realname);
            for (int i = 0; i < users.Count; i++)
            {
               adv_realnameList.Nodes.Add(CreateRealnameNode(users[i].Id,users[i].Realname));
            }
        }
        
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private Node CreateRealnameNode(int id,string realname)
        {
            Node node = new Node(realname);
        
            node.Tag = id;
            return node;
        }
        private Node CreateRoleNode(int roleId, string rolename, string roleValue)
        {
            Node node = new Node(rolename);
            node.Tooltip = "权限:"+roleValue;
            node.Tag = roleId;
            return node;

        }

        private void tb_autoRealname_KeyUp(object sender, KeyEventArgs e)
        {
            InitRealnameList(tb_autoRealname.Text.Trim());
        }

        private void adv_realnameList_NodeClick(object sender, TreeNodeMouseEventArgs e)
        {
            if (isEditing)
            {
                if (DialogResult.Yes == MessageBoxEx.Show("确定放弃编辑？", "放弃保存", MessageBoxButtons.YesNo))
                {
                    
                }
                else 
                {
                    return;
                }
            }
            this.DisableForm();
            Users user=new Users();
            user = bll.GetUserById((int)e.Node.Tag);
            this.lb_id.Text = user.Id.ToString();
            this.tb_realname.Text = user.Realname;
            this.tb_username.Text = user.Username;
            this.comb_role.SelectedValue = user.RoleId;
            btn_edit.Enabled = true;
            btn_delete.Enabled = true;
            btn_saveUser.Enabled = false;
        }

        private void btn_newuser_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                if (DialogResult.Yes == MessageBoxEx.Show("确定放弃编辑？", "放弃保存", MessageBoxButtons.YesNo))
                {

                }
                else
                {
                    return;
                }
            }
            this.EnableForm();
            this.ClearForm();
            this.btn_saveUser.Enabled = true;

        }
        public void SetUserInfo(Users user)
        {
            lb_id.Text = user.Id.ToString();
            tb_realname.Text = user.Realname;
            tb_username.Text = user.Username;
            tb_password.Text = user.Password;
            tb_password2.Text = user.Password;
        }
        private void InitRoleDropdownList()
        {
            roleBll = new RoleBLL();
            comb_role.DataSource = roleBll.GetRoleDropDown();
            

        }
        private void ClearForm()
        {
            this.lb_id.Text = "";
            this.tb_password.Text = "";
            this.tb_password2.Text = "";
            this.tb_realname.Text = "";
            this.tb_username.Text = "";
            this.comb_role.Text = "";
        }
        private void DisableForm()
        {
            this.tb_username.Enabled = false;
            this.tb_realname.Enabled = false;
            this.tb_password.Enabled = false;
            this.tb_password2.Enabled = false;
             this.comb_role.Enabled = false;
            this.isEditing = false;
        }
        private void EnableForm()
        {
            this.tb_username.Enabled = true;
            this.tb_realname.Enabled = true;
            this.tb_password.Enabled = true;
            this.tb_password2.Enabled = true;
            this.comb_role.Enabled = true;
            this.isEditing = true;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            this.EnableForm();
        }

        private void adv_realnameList_Leave(object sender, EventArgs e)
        {
            //MessageBox.Show("aaa");
          
        }

        private void adv_realnameList_AfterNodeDeselect(object sender, AdvTreeNodeEventArgs e)
        {
            btn_edit.Enabled = false;
            btn_delete.Enabled = false;
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (bll.DeleteUserById(int.Parse(lb_id.Text)))
            {
                MessageBoxEx.Show("删除成功", "删除用户");
                this.ClearForm();
            }
            else
            {
                MessageBoxEx.Show("删除失败", "删除用户");
            }
        }

        private void tb_username_TextChanged(object sender, EventArgs e)
        {
            if (isEditing)
            {
                if (!bll.CheckUsername(tb_username.Text))
                {
                    errorProvider1.SetError(tb_username, " 用户名已存在。");
                    highlighter1.SetHighlightColor(tb_username, DevComponents.DotNetBar.Validator.eHighlightColor.Red);
                }
                else
                {
                    errorProvider1.Clear();
                    highlighter1.SetHighlightColor(tb_username, DevComponents.DotNetBar.Validator.eHighlightColor.None);
                }
            }
        }

        private void tb_password2_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void tb_password2_Leave(object sender, EventArgs e)
        {
            if (isEditing)
            {
                if (tb_password.Text != tb_password2.Text)
                {
                    errorProvider1.SetError(tb_password2, " 两次输入的密码不一致。");
                    highlighter1.SetHighlightColor(tb_password2, DevComponents.DotNetBar.Validator.eHighlightColor.Red);
                }
                else
                {
                    errorProvider1.Clear();
                    highlighter1.SetHighlightColor(tb_password2, DevComponents.DotNetBar.Validator.eHighlightColor.None);
                }
            }
        }
        private void SetError(Control c, string ErrMsg)
        {
            errorProvider1.SetError(c, ErrMsg);
            highlighter1.SetHighlightColor(c, DevComponents.DotNetBar.Validator.eHighlightColor.Red);
        }
        private void SetError(Control c, string ErrMsg,string color)
        {
            errorProvider1.SetError(c, ErrMsg);
            switch (color)
            {
                case "red":
                    highlighter1.SetHighlightColor(c, DevComponents.DotNetBar.Validator.eHighlightColor.Red);
                    break;
                case "orange":
                    highlighter1.SetHighlightColor(c, DevComponents.DotNetBar.Validator.eHighlightColor.Orange);
                    break;
            }
           
        }
        private void RemoveError(Control c)
        {
            errorProvider1.Clear();
            highlighter1.SetHighlightColor(c, DevComponents.DotNetBar.Validator.eHighlightColor.None);
        }
        private void btn_saveUser_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tb_username.Text.Trim()))
            {
                this.SetError(tb_username, "用户名不能为空!");
                tb_username.Focus();
                return;
            }
            if (!System.Text.RegularExpressions.Regex.IsMatch(this.tb_realname.Text.Trim(), @"[\u4e00-\u9fa5]+$"))
            {
                this.SetError(tb_realname, "真实姓名不是汉字!", "red");
                tb_realname.Focus();
                return;
            }
            if (tb_password.Text != tb_password2.Text)
            {
                this.SetError(tb_password2, "两次密码输入不一致!", "red");
                tb_password2.Focus();
                return;
            }
            if (string.IsNullOrEmpty(comb_role.Text.ToString()))
            {
                this.SetError(comb_role, "请选择用户角色!", "red");
                comb_role.PerformClick();
                return;
            }
            Users user = new Users(tb_username.Text.Trim(), (int)comb_role.SelectedValue, tb_password.Text, tb_realname.Text.Trim());
            if (bll.AddUser(user))
            {
                MessageBoxEx.Show("添加成功!", "添加用户");
                this.ClearForm();
                this.DisableForm();
            }
            else
            {
                MessageBoxEx.Show("添加失败!", "添加用户");
            }
        }

        private void lb_id_Click(object sender, EventArgs e)
        {

        }

        private void tb_realname_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(this.tb_realname.Text.Trim(), @"[\u4e00-\u9fa5]+$"))
            {
                this.SetError(tb_realname, "真实姓名不是汉字!", "red");
                tb_realname.Focus();

            }
            else
            {
                RemoveError(tb_realname);
            }

        }

        private void comb_role_SelectionChanged(object sender, AdvTreeNodeEventArgs e)
        {
            if (string.IsNullOrEmpty(comb_role.SelectedNode.Text))
            {
                this.SetError(comb_role, "请选择用户角色!", "red");
                comb_role.PerformClick();

            }
            else
            {
                this.RemoveError(comb_role);
            }
        }
        
        
    }
    //public class ComboBoxItem:BaseItem
    //{
    //    private string _text;

    //    public string Text
    //    {
    //        get { return _text; }
    //        set { _text = value; }
    //    }
    //    private object _value;

    //    public object Value
    //    {
    //        get { return _value; }
    //        set { _value = value; }
    //    }
    //    public override string ToString()
    //    {
    //        return this._text;
    //    }
    //    public ComboBoxItem(string text, object value)
    //    {
    //        _text = text;
    //        _value = value;
    //    }
    //}
}