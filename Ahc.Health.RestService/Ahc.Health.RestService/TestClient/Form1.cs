using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestClient.PollingService;

namespace TestClient
{
    public partial class Form1 : Form
    {
        PollingService.ServicePollingClient proxy = new PollingService.ServicePollingClient();
        public Form1()
        {
            InitializeComponent();

        }

        private void btnInsertRule_Click(object sender, EventArgs e)
        {
            try
            {
                Rules rule = new Rules();
                rule.Parameter = txtParam.Text;
                rule.AgeLower = Convert.ToInt16(txtAgeLower.Text);
                rule.AgeUpper = Convert.ToInt16(txtAgeUpper.Text);
                rule.RangeLower = Convert.ToInt16(txtRangeLower.Text);
                rule.RangeUpper = string.IsNullOrEmpty(txtRangeUpper.Text) ? (Int16?)null : Convert.ToInt16(txtRangeUpper.Text);
                rule.Occurence = string.IsNullOrEmpty(txtOccurence.Text) ? (Int16?)null : Convert.ToInt16(txtOccurence.Text);
                rule.TimeWindow = string.IsNullOrEmpty(txtTimeWindow.Text) ? (Int16?)null : Convert.ToInt16(txtTimeWindow.Text);
                rule.Action = txtAction.Text;

                int result = proxy.InsertRules(rule);

                if (result > 0)
                {
                    MessageBox.Show("Success");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed : " + ex.ToString());
            }
            finally
            {
                txtParam.Text = string.Empty;
                txtAgeLower.Text = string.Empty;
                txtAgeUpper.Text = string.Empty;
                txtRangeUpper.Text = string.Empty;
                txtRangeUpper.Text = string.Empty;
                txtOccurence.Text = string.Empty;
                txtTimeWindow.Text = string.Empty;
            }
            //proxy.InsertRulesCompleted += new EventHandler<ServicePollingClient.InsertRulesCompletedEventArgs>(proxy_Completed);
        }

        private void btnRule_Click(object sender, EventArgs e)
        {
            PersonDetails user = proxy.GetUser(Convert.ToInt32(txtRow.Text.Trim()));
            proxy.ApplyRule(user);
        }

        //private void proxy_Completed(object sender, InsertRulesCompletedEventArgs e)
        //{
        //    MessageBox.Show(e.Result);
        //}
    }
}
