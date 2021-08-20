using Newtonsoft.Json;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Qiniu2
{
    public class QiniuUploadHelper
    {
        public static string UploadFile(string extend_name, byte[] image_data, string type = "truck")
        {
            string bucketName = "dl700t";
            PutPolicy policy = new PutPolicy() { Scope = bucketName };
            policy.SetExpires(100);

            Mac mac = new Mac("BTthnHsa-0TyJY73uU1LQ8QBYKb37lQTB_EQcwX7", "Lj75xvtl75hp9GnINEJ7O5vCq1Mbyq-piteuvDQ9");
            string token = Auth.CreateUploadToken(mac, JsonConvert.SerializeObject(policy));

            // 方式1：使用UploadManager
            //默认设置 Qiniu.Common.Config.PUT_THRESHOLD = 512*1024;
            //可以适当修改,UploadManager会根据这个阈值自动选择是否使用分片(Resumable)上传    
            Config config = new Config();
            config.Zone = Zone.ZONE_CN_South;

            UploadManager um = new UploadManager(config);

            // 文件名
            string key = type + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + Guid.NewGuid().ToString("N") + extend_name;
            var result = um.UploadData(image_data, key, token, new PutExtra());
            if (result.Code == 200)
            {
                // 图片地址
                string pic_url = "https://ssoimg.700t.com/" + key;

                return pic_url;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}

