using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

//using System.Windows.Forms;

namespace UserInterfaces._14._06._2021
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            showtreeview();
            //Kampus kampus = new Kampus();
            //kampus.isim = "kdhsbf";
            
        }
        SqlConnection connection = new SqlConnection("Data Source=.;Initial Catalog=dbdugumleryeni;Integrated Security=True;Pooling=False");

        string[] strPath2StyleSheetsFolder = System.IO.Directory.GetFiles(Application.StartupPath + "\\StyleSheets\\");
        private void AddStyleSheetsToCombo()
        {
            ComboBoxStyleSheets.Items.Add("<<Theme Select>>");

            //GetFiles metodu dosyaları temsil eder. Belirtilen Dizindeki Dosyaları Dizi olarak döndürür
            

            foreach (var item in strPath2StyleSheetsFolder)
            {
                if (item.EndsWith(".qss"))
                {
                    ComboBoxStyleSheets.Items.Add(System.IO.Path.GetFileName(item));
                }


            }

        }

        private void ComboBoxStyleSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            string StyleSheetFile = strPath2StyleSheetsFolder + ComboBoxStyleSheets.SelectedItem.ToString();

            //style
           
            this.SetStyle(ControlStyles.Selectable,true);
            
            //StreamReader sr = new StreamReader(StyleSheetFile);
            //string firstLine = sr.Read()
            //string otherLines = sr.ReadToEnd();

            //sr.Close();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            AddStyleSheetsToCombo();
            treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
            toolStripButton1.Enabled = false;

        }

        dbdugumleryeniEntities datacontext = new dbdugumleryeniEntities();
        
        
        private void showtreeview()
        {
            // rootno adında variable değişken oluşturduk.
            // oluşturduğumuz datacontext sınıfının nesnesinin altındaki adreslerchar tablosundaki 
            //nereyeait 0 olanları al diye root adında bir sorgu oluşturduk.
            var rootno = datacontext.adresleryeni.Where(a => a.nereyeait == "0");

            foreach (var root in rootno)// root node oluşturuldu.
            {
                // nereyeait sıfır olanların sorgusuna ait lokasyonadi alanındaki lokasyon adlarını 
                // treenode  nesnesinden oluşturduğumuz rootnode a aktardık.
                TreeNode rootnode = new TreeNode(root.lokasyonadi);
                adresleryeni adr = root as adresleryeni;
                var nodeno = datacontext.adresleryeni.Where(a => a.nereyeait == adr.lokasyonid);
                foreach (var node in nodeno)// rootnode>node oluşturuldu.
                {
                    TreeNode nodenode = new TreeNode(node.lokasyonadi);
                    adresleryeni adr2 = node as adresleryeni;
                    var childno = datacontext.adresleryeni.Where(a => a.nereyeait == adr2.lokasyonid);
                    foreach (var child in childno)// rootnode-->node-->>childnode  oluşturuldu.
                    {
                        TreeNode childnode = new TreeNode(child.lokasyonadi);
                        adresleryeni adr3 = child as adresleryeni;
                        var grandchildno = datacontext.adresleryeni.Where(a => a.nereyeait == adr3.lokasyonid);
                        foreach (var grandchild in grandchildno) //rootnode-- > node-- > childnode ->granchildnode  oluşturuldu.
                        {
                            TreeNode grandchildnode = new TreeNode(grandchild.lokasyonadi);
                            adresleryeni adr4 = grandchild as adresleryeni;
                            var mastergrandchildno = datacontext.adresleryeni.Where(a => a.nereyeait == adr4.lokasyonid);
                            foreach (var mastergrandchild in mastergrandchildno)//rootnode-- > node-- > childnode ->granchildnode-> mastergrandchildnode  oluşturuldu.
                            {
                                TreeNode mastergrandchildnode = new TreeNode(mastergrandchild.lokasyonadi);

                                grandchildnode.Nodes.Add(mastergrandchildnode);

                                listVievKonumId.Add(mastergrandchild.lokasyonid);// lokasyonid değeri listeye eklendi
                                listVievKonumAdi.Add(mastergrandchild.lokasyonadi);// lokasyonadi değeri listeye eklendi
                            }

                            childnode.Nodes.Add(grandchildnode);
                            listVievKonumAdi.Add(grandchild.lokasyonadi);
                            listVievKonumId.Add(grandchild.lokasyonid);
                        }

                        nodenode.Nodes.Add(childnode);
                        listVievKonumAdi.Add(child.lokasyonadi);
                        listVievKonumId.Add(child.lokasyonid);
                    }

                    rootnode.Nodes.Add(nodenode);
                    listVievKonumAdi.Add(node.lokasyonadi);
                    listVievKonumId.Add(node.lokasyonid);
                }

                treeView1.Nodes.Add(rootnode);
                listVievKonumAdi.Add(root.lokasyonadi);
                listVievKonumId.Add(root.lokasyonid);
            }

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Text != "")
            {
                toolStripButton1.Enabled = true;
            }

            else
            {
                toolStripButton1.Enabled = false;
            }




            // listVievKonumAdi listesine eklenen konum adı değerleri saydırıldı.
            for (int konumadi = 0; konumadi < listVievKonumAdi.Count; konumadi++)
            {
                // tıklanan node un  text değeri ile konumid değeri esitse aşağıdaki işlemi yapsın
                if (e.Node.Text == listVievKonumAdi[konumadi].ToString())
                {
                    listView1.Items.Clear();

                    // SplitOfId metodu çağrıldı ve listVievKonumId[konumadi] listesine konumadi inci değeri string olaral gönderildi
                    SplitOfId(listVievKonumId[konumadi].ToString());

                }

            }
        }


        ArrayList listVievKonumAdi = new ArrayList();
        ArrayList listVievKonumId = new ArrayList();
        ArrayList listVievKonumId2 = new ArrayList();

        List<string> CihazId = new List<string>();
        ListViewItem itemlcname;
        ListViewItem itemlcname1;
        ListViewItem itemlcname2;
        ListViewItem itemlcname3;
        ListViewItem itemlcname4;
        ListViewItem itemlcname5;


        int countRootEkle = 1;
        string defaultKonumId = "1000000000";

        bool rootKontrolLokasyonAdi = false;
        bool rootKontrolLokasyonId = false;

        int idArtırmaRoot = 11;
        int idArtırmaNode = 00;
        //int idArtırmaChild = 00;
        //int idArtırmaGrandChild = 00;
        //int idArtırmaMasterGrandChild = 00;

        
        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            rootKontrolLokasyonId = false;
            string defaultKonumIdSubString1 = defaultKonumId.Substring(0, 2);
            string defaultKonumIdSubString2 = defaultKonumId.Substring(2, 8);
            //string defaultKonumIdSubString3 = defaultKonumId.Substring(4, 2);
            //string defaultKonumIdSubString4 = defaultKonumId.Substring(6, 2);
            //string defaultKonumIdSubString5 = defaultKonumId.Substring(8, 2);

            //int intdefaultKonumIdSubString1 = Convert.ToInt32(defaultKonumIdSubString1);
            var lokasyonid = datacontext.adresleryeni.Select(a=>a.lokasyonid);

            foreach (var item in lokasyonid)
            {
                listVievKonumId2.Add(item);
            }

            //if (listVievKonumId.Contains(idArtırmaRoot.ToString() + defaultKonumIdSubString2))

            for (int i = 0; i < listVievKonumId.Count; i++)
            {
                //listView1.Items.Add(listVievKonumId[i].ToString());

                //if (listVievKonumId[i].ToString().StartsWith(idArtırmaRoot.ToString()))
                if (listVievKonumId[i].ToString().Contains(idArtırmaRoot.ToString() + defaultKonumIdSubString2))// StartsWith(idArtırmaRoot.ToString()))
                //if (listVievKonumId[i].ToString().Contains(listVievKonumId2[i].ToString()))// StartsWith(idArtırmaRoot.ToString()))
                {
                    connection.Open();
                    SqlCommand deletesorgu = new SqlCommand("DELETE FROM adresleryeni where lokasyonid='" + idArtırmaRoot.ToString() + "" + defaultKonumIdSubString2 + "'", connection);
                    
                    //SqlCommand selectsorgu = new SqlCommand("select * FROM binalar where kamousıs='" + idArtırmaRoot.ToString() + "" + defaultKonumIdSubString2 + "'", connection);
                    deletesorgu.ExecuteNonQuery();
                    //MessageBox.Show("Node Başarıyla Silindi");
                    connection.Close();

                    connection.Open();

                    SqlCommand eklekomutu1 = new SqlCommand("insert adresleryeni (lokasyonadi,lokasyonid,nereyeait) " +
                        "values('Root " + Convert.ToString(countRootEkle) + "','" + idArtırmaRoot.ToString() + "" + defaultKonumIdSubString2 + "','0') ", connection);
                    eklekomutu1.ExecuteNonQuery();
                    connection.Close();

                    //connection.Close();
                    treeView1.Nodes.Clear();
                    showtreeview();

                    
                    countRootEkle++;
                    idArtırmaRoot++;

                }

               
            }

         
            connection.Open();

            SqlCommand eklekomutu = new SqlCommand("insert adresleryeni (lokasyonadi,lokasyonid,nereyeait) " +
                "values('Root " + Convert.ToString(countRootEkle) + "','" + idArtırmaRoot.ToString() + "" + defaultKonumIdSubString2 + "','0') ", connection);
            eklekomutu.ExecuteNonQuery();
            connection.Close();

            treeView1.Nodes.Clear();
            showtreeview();
            
            label1.Text = idArtırmaRoot.ToString();


        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
               
                connection.Open();
                SqlCommand deletesorgu = new SqlCommand("DELETE FROM adresleryeni where lokasyonadi='"+treeView1.SelectedNode.Text+"'", connection);
                deletesorgu.ExecuteNonQuery();
                MessageBox.Show("Node Başarıyla Silindi");
            
           
                connection.Close();
                treeView1.Nodes.Clear();
                showtreeview();

                toolStripButton1.Enabled = false;
            }
            catch (NullReferenceException errmsg)
            {
                //connection.Close();
                //MessageBox.Show(errmsg.Message);
                return;
            }
            
        }

        string stringitemkonumid;

        string SplitString2, SplitString3, SplitString4, SplitString5;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void AddChildNodeButton_Click(object sender, EventArgs e)
        {
            idArtırmaNode++;
            string defaultKonumIdSubString1 = defaultKonumId.Substring(2, 8);
            string defaultKonumIdSubString2 = defaultKonumId.Substring(4, 6);
            connection.Open();
            SqlCommand eklekomutu1 = new SqlCommand("insert adresleryeni (lokasyonadi,lokasyonid,nereyeait) " +
                       "values('Child Node " + Convert.ToString(countRootEkle) + "','" + idArtırmaRoot.ToString() + ""+ idArtırmaNode+"" + defaultKonumIdSubString2 + "'," +
                       "'" + idArtırmaRoot.ToString() + "" + defaultKonumIdSubString1 + "') ", connection);
            eklekomutu1.ExecuteNonQuery();
            connection.Close();

            //connection.Close();
            treeView1.Nodes.Clear();
            showtreeview();

            countRootEkle++;
            idArtırmaRoot++;

        }





        //listviewkonumid listesinden gelen  konumid = itemkonumid olur.
        private void SplitOfId(string itemkonumid)
        {
            listView1.Items.Clear();


            // itemkonumid değeri stringitemkonum id değişkenine atandı.
            stringitemkonumid = itemkonumid;
            // stringitemkonum değerinin 2. indexinden sonra 2 basamak alıp splitstring2 değişkenine atandı.node temsil eder
            SplitString2 = stringitemkonumid.Substring(2, 2);
            // stringitemkonum değerinin 4. indexinden sonra 2 basamak alıp splitstring3 değişkenine atandı. childnode temsil eder
            SplitString3 = stringitemkonumid.Substring(4, 2);
            // stringitemkonum değerinin 6. indexinden sonra 2 basamak alıp splitstring4 değişkenine atandı. grandchildnode temsil eder
            SplitString4 = stringitemkonumid.Substring(6, 2);
            // stringitemkonum değerinin 8. indexinden sonra 2 basamak alıp splitstring5 değişkenine atandı. mastergrandchildnode temsil eder.
            SplitString5 = stringitemkonumid.Substring(8, 2);

            // datacontext nesnesini locationdevichar tablosunda cihazid değerinin ilk 2 sayısı ile
            // itemkonumid değerinin ilk 2 basamağı eşitse bu durumu where sorgusuyla splidid variable değişkenine atandı.
            var splitid = datacontext.locationdevice.Where(a => a.cihazid.Substring(0, 2) == itemkonumid.Substring(0, 2));
            ////


            string sifirlar = "00";

            // splid id değişkeninin içinde barındıran cihazid değerleri CihazId listesine eklendi.
            foreach (var itemad in splitid)
            {
                CihazId.Add(itemad.ToString());

            }
            //listView1.Items.Remove(itemlcname);

            //splidid değerininiçindeki cihaz id değerleri saydırıldı.
            foreach (var item in splitid)
            {
                // cihaz id değerini barındıran 6 adet Listviewitem nesnesi tamımlandı
                itemlcname = new ListViewItem(item.cihazid.ToString().Substring(0, 12));
                itemlcname1 = new ListViewItem(item.cihazid.ToString().Substring(0, 12));
                itemlcname2 = new ListViewItem(item.cihazid.ToString().Substring(0, 12));
                itemlcname3 = new ListViewItem(item.cihazid.ToString().Substring(0, 12));
                itemlcname4 = new ListViewItem(item.cihazid.ToString().Substring(0, 12));
                itemlcname5 = new ListViewItem(item.cihazid.ToString().Substring(0, 12));



                if (itemkonumid.ToString().Contains(item.cihazid.ToString().Substring(0, 2)) && SplitString2 == sifirlar && SplitString3 == sifirlar && SplitString4 == sifirlar && SplitString5 == sifirlar)
                {


                    if (listView1.Items.Contains(itemlcname))
                    {
                        listView1.Items.Remove(itemlcname);

                        itemlcname1.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname1);
                    }

                    else
                    {
                        itemlcname1.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname1);
                    }

                }

                if (itemkonumid.ToString().Contains(item.cihazid.ToString().Substring(0, 4)) && SplitString3 == sifirlar && SplitString4 == sifirlar && SplitString5 == sifirlar)
                {
                    if (listView1.Items.Contains(itemlcname))
                    {
                        listView1.Items.Remove(itemlcname);
                        listView1.Items.Remove(itemlcname1);

                        itemlcname2.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname2);
                    }

                    else
                    {
                        itemlcname.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname);
                    }
                }

                if (itemkonumid.ToString().Contains(item.cihazid.ToString().Substring(0, 6)) && SplitString4 == sifirlar && SplitString5 == sifirlar)
                {
                    if (listView1.Items.Contains(itemlcname))
                    {
                        listView1.Items.Remove(itemlcname);
                        listView1.Items.Remove(itemlcname1);
                        listView1.Items.Remove(itemlcname2);

                        itemlcname3.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname3);

                    }
                    else
                    {
                        itemlcname3.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname3);
                    }

                }

                if (itemkonumid.ToString().Contains(item.cihazid.ToString().Substring(0, 8)) && SplitString5 == sifirlar)
                {
                    if (listView1.Items.Contains(itemlcname))
                    {
                        listView1.Items.Remove(itemlcname);
                        listView1.Items.Remove(itemlcname1);
                        listView1.Items.Remove(itemlcname2);
                        listView1.Items.Remove(itemlcname3);

                        itemlcname4.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname4);
                    }

                    else
                    {
                        itemlcname.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname);
                    }
                }

                if (itemkonumid.ToString().Contains(item.cihazid.ToString().Substring(0, 10)))
                {
                    if (listView1.Items.Contains(itemlcname))
                    {
                        listView1.Items.Remove(itemlcname);
                        listView1.Items.Remove(itemlcname1);
                        listView1.Items.Remove(itemlcname2);
                        listView1.Items.Remove(itemlcname3);
                        listView1.Items.Remove(itemlcname4);


                        itemlcname5.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));
                        listView1.Items.Add(itemlcname5);

                    }

                    else
                    {
                        itemlcname.SubItems.Add(item.cihazadi.ToString().Substring(0, 14));

                        listView1.Items.Add(itemlcname);
                    }
                }
            }
        }
    }
}
