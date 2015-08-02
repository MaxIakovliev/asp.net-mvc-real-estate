using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RealtyDomainObjects;

namespace HtmlParser.DBLayer
{
    public class SqlObjectManipulation
    {
        string connStr = string.Empty;
        public SqlObjectManipulation(string connStr)
        {
            this.connStr = connStr;
        }

        public int Mysql_File_Save(int PropertyObject, int FileSize, string FileName, string Content_Type,
            int Height, int Width, byte[] Image, byte[] ImagePreview, bool IsDeleted)
        {
            int result = -1;
            using (MySql.Data.MySqlClient.MySqlConnection oConn =
                new MySql.Data.MySqlClient.MySqlConnection(this.connStr))
            {
                oConn.Open();

                MySql.Data.MySqlClient.MySqlCommand cmd = oConn.CreateCommand();
                cmd.Connection = oConn;


                //Add new 
                //oCommand.CommandText = "insert into cust_file(customer_id, filename, filedata, contenttype, length) " +
                //    "values( ?in_customer_id, ?in_filename, ?in_filedata, ?in_contenttype, ?in_length)";

                //INSERT INTO myrealty.images (id, img) VALUES (<INT(11)>, <LONGBLOB>);
                cmd.CommandText = @"SET GLOBAL max_allowed_packet=16*1024*1024; 
INSERT INTO ObjectImages (PropertyObject_Id, FileSize, FileName, Content_Type, Height, Width,
Image, ImagePreview, IsDeleted) VALUES (?PropertyObject, ?FileSize, ?FileName, ?Content_Type, ?Height, ?Width,
?Image, ?ImagePreview, ?IsDeleted); select last_insert_id();";


                //oCommand.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("?PropertyObject", PropertyObject);

                cmd.Parameters.Add("?FileSize", FileSize);

                cmd.Parameters.Add("?FileName", FileName);

                cmd.Parameters.Add("?Content_Type", Content_Type);

                cmd.Parameters.Add("?Height", Height);

                cmd.Parameters.Add("?Width", Width);

                cmd.Parameters.Add("?Image", MySql.Data.MySqlClient.MySqlDbType.LongBlob);
                cmd.Parameters["?Image"].Value = Image;

                cmd.Parameters.Add("?ImagePreview", MySql.Data.MySqlClient.MySqlDbType.LongBlob);
                cmd.Parameters["?ImagePreview"].Value = ImagePreview;

                cmd.Parameters.Add("?IsDeleted", IsDeleted);

                result = Convert.ToInt32(cmd.ExecuteScalar());
                oConn.Close();
            }
            return result;
        }



        public int AddPropertyObject(PropertyObject po)
        {
            int result = -1;
            using (MySql.Data.MySqlClient.MySqlConnection oConn =
                new MySql.Data.MySqlClient.MySqlConnection(this.connStr))
            {
                oConn.Open();

                MySql.Data.MySqlClient.MySqlCommand cmd = oConn.CreateCommand();
                cmd.Connection = oConn;

                cmd.CommandText = @"INSERT INTO PropertyObjects (City_Id, CityDistrict_Id, PropertyType_Id, PropertyAction_Id,
Title,PropertyDescription,RoomCount,TotalSpace,LivingSpace,KitchenSpace,BuildingTypeName_Id,Floor,CountFloors,IsNewBuilding,BalconAvailable,
BalconSpace,isBalconGlassed,ContactName,Price,Currency_Id,PriceForTypeName_Id,NoCommission,Phone1,Phone2,Phone3,Periods_Id,CreatedDate,SourceUrl,
UserOwner_Id,DistanceToCity,WCType_Id,CountPhotos,CommercialPropertyType_Id,ServiceType_Id,IsActive,IsDeleted,DeletedDate,LinkOfObjectGrab) values (?City, ?CityDistrict, ?PropertyType, 
?PropertyAction,
?Title,?PropertyDescription,?RoomCount,?TotalSpace,?LivingSpace,?KitchenSpace,?BuildingTypeName,?Floor,?CountFloors,?IsNewBuilding,?BalconAvailable,
?BalconSpace,?isBalconGlassed,?ContactName,?Price,?Currency,?PriceForTypeName,?NoCommission,?Phone1,?Phone2,?Phone3,?Periods,?CreatedDate,?SourceUrl,
?UserOwner,?DistanceToCity,?WCType,?CountPhotos,?CommercialPropertyType,?ServiceType,?IsActive,?IsDeleted,?DeletedDate,?LinkOfObjectGrab);
select last_insert_id();";

                cmd.Parameters.Add("?City", (po.City != null ? po.City.Id.ToString() : null));

                cmd.Parameters.Add("?CityDistrict", (po.CityDistrict != null ? po.CityDistrict.Id.ToString() : null));

                cmd.Parameters.Add("?PropertyType", (po.PropertyType != null ? po.PropertyType.Id.ToString() : null));

                cmd.Parameters.Add("?PropertyAction", (po.PropertyAction != null ? po.PropertyAction.Id.ToString() : null));

                cmd.Parameters.Add("?Title", po.Title);

                cmd.Parameters.Add("?PropertyDescription", po.PropertyDescription);

                cmd.Parameters.Add("?RoomCount", po.RoomCount);

                cmd.Parameters.Add("?TotalSpace", po.TotalSpace);

                cmd.Parameters.Add("?LivingSpace", po.LivingSpace);

                cmd.Parameters.Add("?KitchenSpace", po.KitchenSpace);

                cmd.Parameters.Add("?BuildingTypeName", (po.BuildingTypeName != null ? po.BuildingTypeName.Id.ToString() : null));

                cmd.Parameters.Add("?Floor", po.Floor);

                cmd.Parameters.Add("?CountFloors", po.CountFloors);

                cmd.Parameters.Add("?IsNewBuilding", po.IsNewBuilding);

                cmd.Parameters.Add("?BalconAvailable", po.BalconAvailable);

                cmd.Parameters.Add("?BalconSpace", po.BalconSpace);

                cmd.Parameters.Add("?isBalconGlassed", po.isBalconGlassed);

                cmd.Parameters.Add("?ContactName", po.ContactName);

                cmd.Parameters.Add("?Price", po.Price);

                cmd.Parameters.Add("?Currency", (po.Currency != null ? po.Currency.Id.ToString() : null));

                cmd.Parameters.Add("?PriceForTypeName", (po.PriceForTypeName != null ? po.PriceForTypeName.Id.ToString() : null));

                cmd.Parameters.Add("?NoCommission", po.NoCommission);

                cmd.Parameters.Add("?Phone1", po.Phone1);
                cmd.Parameters.Add("?Phone2", po.Phone2);
                cmd.Parameters.Add("?Phone3", po.Phone3);

                cmd.Parameters.Add("?Periods", (po.Periods != null ? po.Periods.Id.ToString() : null));

                cmd.Parameters.Add("?CreatedDate", po.CreatedDate);

                cmd.Parameters.Add("?SourceUrl", po.SourceUrl);

                cmd.Parameters.Add("?UserOwner", (po.UserOwner != null ? po.UserOwner.Id.ToString() : null));

                cmd.Parameters.Add("?DistanceToCity", po.DistanceToCity);

                cmd.Parameters.Add("?WCType", (po.WCType!=null?po.WCType.Id.ToString():null));

                cmd.Parameters.Add("?CountPhotos", po.CountPhotos);

                cmd.Parameters.Add("?CommercialPropertyType", (po.CommercialPropertyType != null ? po.CommercialPropertyType.Id.ToString() : null));

                cmd.Parameters.Add("?ServiceType", (po.ServiceType != null ? po.ServiceType.Id.ToString() : null));

                cmd.Parameters.Add("?IsActive", po.IsActive);

                cmd.Parameters.Add("?IsDeleted", po.IsDeleted);

                cmd.Parameters.Add("?DeletedDate", po.DeletedDate);

                cmd.Parameters.Add("?LinkOfObjectGrab", po.LinkOfObjectGrab);


                result = Convert.ToInt32(cmd.ExecuteScalar());
                oConn.Close();

            }
            return result;
        }
    }
}
