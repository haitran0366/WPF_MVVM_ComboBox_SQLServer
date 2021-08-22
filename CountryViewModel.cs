using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPF_MVVM_ComboBox_SQLServer
{
    public class CountryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string hello;
        public string Hello
        {
            get { return hello; }
            set
            {
                hello = value;
                NotifyPropertyChanged("Hello");
            }
        }

        public List<CountryModel> countryList;
        public List<CountryModel> CountryList
        {
            get { return countryList; }
            set
            {
                countryList = value;
            }
        }

        private CountryModel selectedCountry;
        public CountryModel SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                selectedCountry = value;
                NotifyPropertyChanged("SelectedCountry");
                Hello = SelectedCountry.greeting;
            }
        }

        private string CnnStr = Properties.Settings.Default.WPF_Connection;
        public CountryViewModel()
        {
            DataSet ds = new DataSet();
            using(SqlConnection connection = new SqlConnection(CnnStr))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand("select * from tblCountry", connection);
                dataAdapter.Fill(ds);
            }

            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            countryList = new List<CountryModel>();
            for(int i=0; i<dt.Rows.Count;i++)
            {
                DataRow dr = dt.NewRow();
                dr = dt.Rows[i];
                CountryModel countryModel = new CountryModel();
                countryModel.countryID = (int)dr["countryID"];
                countryModel.countryName = dr["countryName"].ToString();
                countryModel.greeting = dr["greeting"].ToString();

                CountryList.Add(countryModel);
            }

            SelectedCountry = countryList[0];
        }
    }
}
