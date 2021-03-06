using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NSWI152
{
    public partial class Default : System.Web.UI.Page
    {
        protected void GoButton_Click(object sender, EventArgs e)
        {
            if (MyFileUpload.HasFile)
            {
                var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageAccountConnectionString"].ConnectionString);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var containerReference = blobClient.GetContainerReference("test"); // name of your container

                var blobReference = containerReference.GetBlockBlobReference(MyFileUpload.FileName);
                blobReference.UploadFromStream(MyFileUpload.FileContent);
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageAccountConnectionString"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var containerReference = blobClient.GetContainerReference("test");

            var blobs = containerReference.ListBlobs();
            FilesRepeater.DataSource = blobs;
            FilesRepeater.DataBind();
        }

        protected void FileLink_Command(object sender, CommandEventArgs e)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageAccountConnectionString"].ConnectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();

            var blobReference = new CloudBlockBlob(new Uri((string)e.CommandArgument), blobClient);
            blobReference.DownloadToStream(Response.OutputStream);
        }
        protected void SendToQueueButton_Click(object sender, EventArgs e)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageAccountConnectionString"].ConnectionString);

            var queueClient = storageAccount.CreateCloudQueueClient();
            var queueReference = queueClient.GetQueueReference("test"); // your queue name

            var message = new CloudQueueMessage(QueueMessageTB.Text);
            queueReference.AddMessage(message);
        }
        protected void GetMessageButton_Click(object sender, EventArgs e)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageAccountConnectionString"].ConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queueReference = queueClient.GetQueueReference("test"); // your queue name

            var message = queueReference.GetMessage();
            if (message != null)
            {
                QueueMessageLb.Text = message.AsString;
                queueReference.DeleteMessage(message);
            }
            else
            {
                QueueMessageLb.Text = "No message returned...";
            }
        }
    }
}