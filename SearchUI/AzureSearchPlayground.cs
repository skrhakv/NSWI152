using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SearchUI
{
    public partial class AzureSearchPlayground : Form
    {
        public AzureSearchPlayground()
        {
            InitializeComponent();
        }

        private void ButtonIndex_Click(object sender, EventArgs e)
        {
            Article article = new Article
            {
                Title = InputTitle.Text,
                Category = InputCategory.Text,
                Text = InputText.Text
            };

            // TODO: index new item
            using (var serviceClient = new SearchServiceClient("azure-search-test11", new SearchCredentials("A70A4FD8B31C177C6A9B86FDD4AD82C6")))
            {
                var actions = new IndexAction<Article>[]
                {
                    IndexAction.MergeOrUpload(article)
                };

                var batch = IndexBatch.New(actions);

                ISearchIndexClient indexClient = serviceClient.Indexes.GetClient("article");
                indexClient.Documents.Index(batch);
            }
        }

        private void ButtonSearch_Click(object sender, EventArgs e)
        {
            // TODO: get results from DB
            using (var indexClient = new SearchIndexClient("azure-search-test11", "article", new SearchCredentials("A70A4FD8B31C177C6A9B86FDD4AD82C6")))
            {
                var parameters = new SearchParameters()
                {
                    // Select = new[] { "" }
                };

                var results = indexClient.Documents.Search<Article>(textBox1.Text, parameters);

                ResultGrid.DataSource = results.Results.Select(x => new
                {
                    Score = x.Score,
                    Title = x.Document.Title,
                    Category = x.Document.Category,
                    Text = x.Document.Text
                }).OrderByDescending(x => x.Score).ToList();
            }
        }
    }
}
