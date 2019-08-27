using System;
using RestSharp;
using HtmlAgilityPack;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.IO;
using TikaOnDotNet;
using TikaOnDotNet.TextExtraction;


namespace TUF
{
    
    class crawler
    {
       public  static ArrayList hyperLinks = new ArrayList();
        static ArrayList visited = new ArrayList();
        static ArrayList toBeVisited = new ArrayList();
        static ArrayList totalLinks = new ArrayList();
        static int count = 0;
       
        public static void crawlerinit()
        {
            toBeVisited.Add(@"http://nu.edu.pk/");
        }
        public static void searchByKeyword(string keyword, System.Windows.Forms.ListBox L)
        {
            if (count > 15)
            {
                return;
            }
            var restClient = new RestClient();
            if (toBeVisited.Count != 0)
            {
                restClient.BaseUrl = validUrl(toBeVisited[0].ToString());

                visited.Add(toBeVisited[0]);
                toBeVisited.Remove(toBeVisited[0]);

                if (restClient != null)
                {
                    
                        var request = new RestRequest(Method.GET);                 //Requesting for the URL
                    request.AddHeader("Accept", "*/*");
                    IRestResponse response = restClient.Execute(request);
                    
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)   //Filtering successful links
                    {
                        string fileName;

                        fileName = response.ResponseUri.Segments[response.ResponseUri.Segments.Length - 1];    //getting the filename from the last segment/portion of the URL

                        if (response.ContentType.Contains("text/html"))
                        {
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(response.Content);                           //Loading the DOM(Html Document) in HtmlDocument
                            HtmlNode node = doc.DocumentNode;
                            var linkCollection = node.SelectNodes("//a");            //Saving all the anchor tags to the collection
                            if (linkCollection != null)
                            {
                                foreach (HtmlNode linkNode in linkCollection)
                                {
                                    string link = linkNode.Attributes["href"].Value;   //Getting hyperlinks from an HTML Document
                                    FilterHttpLinks(link.ToLower());
                                }

                            }

                            removeUnnecessaryLinks(ref totalLinks);

                            NewLinksAdded();

                            if (response.Content.ToLower().Contains(keyword))                      //Searching for the keyword
                            {
                                
                                    hyperLinks.Add(restClient.BaseUrl.ToString());
                                    L.Items.Add("[" + count + "]" + " " + hyperLinks[count] + " " + "\n");
                                    count++;
                                
                            }

                        }
                        }
                    }
                }
            
            searchByKeyword(keyword,L);
        }
        public static void DownloadType(string type)
        {
           
            var restClient = new RestClient();
            if (toBeVisited.Count != 0)
            {
                restClient.BaseUrl = validUrl(toBeVisited[0].ToString());

                visited.Add(toBeVisited[0]);
                toBeVisited.Remove(toBeVisited[0]);

                if (restClient != null)
                {

                    var request = new RestRequest(Method.GET);                 //Requesting for the URL
                    request.AddHeader("Accept", "*/*");
                    IRestResponse response = restClient.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)   //Filtering successful links
                    {
                        string fileName;

                        fileName = response.ResponseUri.Segments[response.ResponseUri.Segments.Length - 1];    //getting the filename from the last segment/portion of the URL

                        if (response.ContentType.Contains("text/html"))
                        {
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(response.Content);                           //Loading the DOM(Html Document) in HtmlDocument
                            HtmlNode node = doc.DocumentNode;
                            var linkCollection = node.SelectNodes("//a");
                            var jpgCollection = node.SelectNodes("//img");   //Saving all the anchor tags to the collection
                            if (linkCollection != null || jpgCollection != null)
                            {
                                if ((type == "jpg" || type == "png") && (jpgCollection != null))
                                {
                                    foreach (HtmlNode linkNode in jpgCollection)
                                    {
                                        string link = linkNode.Attributes["src"].Value;   //Getting image hyperlinks from an HTML Document
                                        FilterHttpLinks(link.ToLower());
                                    }
                                }
                                foreach (HtmlNode linkNode in linkCollection)
                                {
                                    string link = linkNode.Attributes["href"].Value;   //Getting hyperlinks from an HTML Document
                                    FilterHttpLinks(link.ToLower());
                                }

                            }

                            removeUnnecessaryLinks(ref totalLinks);

                            NewLinksAdded();
                        }

                            DownloadFiles(response.RawBytes, fileName, type);

                        
                    }
                }
            }

            DownloadType(type);
        }
        public static void searchByKeywordtype(string keyword,string type, System.Windows.Forms.ListBox L)
        {
            if (count > 15)
            {
                return;
            }
            var restClient = new RestClient();
            if (toBeVisited.Count != 0)
            {
                restClient.BaseUrl = validUrl(toBeVisited[0].ToString());

                visited.Add(toBeVisited[0]);
                toBeVisited.Remove(toBeVisited[0]);

                if (restClient != null)
                {

                    var request = new RestRequest(Method.GET);                 //Requesting for the URL
                    request.AddHeader("Accept", "*/*");
                    IRestResponse response = restClient.Execute(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)   //Filtering successful links
                    {
                        string fileName;

                        fileName = response.ResponseUri.Segments[response.ResponseUri.Segments.Length - 1];    //getting the filename from the last segment/portion of the URL

                        if (response.ContentType.Contains("text/html"))
                        {
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(response.Content);                           //Loading the DOM(Html Document) in HtmlDocument
                            HtmlNode node = doc.DocumentNode;
                            var linkCollection = node.SelectNodes("//a");            //Saving all the anchor tags to the collection
                            if (linkCollection != null)
                            {
                                foreach (HtmlNode linkNode in linkCollection)
                                {
                                    string link = linkNode.Attributes["href"].Value;   //Getting hyperlinks from an HTML Document
                                    FilterHttpLinks(link.ToLower());
                                }

                            }

                            removeUnnecessaryLinks(ref totalLinks);

                            NewLinksAdded();

                        }
                        searchInFiles(response.RawBytes, fileName, keyword, restClient.BaseUrl.ToString(), L,type);

                    }
                    }
                }
            

            searchByKeywordtype(keyword,type, L);
        }


        static Uri validUrl(string link)            //Checking if the URL is valid or not
        {
            Uri uriResult;

            if (Uri.TryCreate(toBeVisited[0].ToString(), UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
            {
                return new Uri(toBeVisited[0].ToString());
            }
            else
                return null;
        }
        static void FilterHttpLinks(string link)                      //Filtering unnecesssary links
        {
            if (!link.Contains("http") && link != "/" && link != "" && !link.Contains("void(0)") && !link.Contains("#"))
            {
                totalLinks.Add(link.Insert(0, @"http://nu.edu.pk"));
            }
        }
        static void NewLinksAdded()                   //New links added to toBeVisited list
        {
            foreach (string str in totalLinks)
            {
                toBeVisited.Add(str);
            }
        }
        static void DownloadFiles(byte[] rawFile, string fileName, string type)                  //Save the download file to the disk
        {
            string fileExtension = Path.GetExtension(fileName);                 //Gets the extension for the downloaded file from its name
            if ("." + type == fileExtension)
            {
                string filePath = Directory.GetCurrentDirectory() + "\\Downloads\\";
                Directory.CreateDirectory(filePath);
                File.WriteAllBytes(filePath + fileName, rawFile);
                System.Windows.Forms.MessageBox.Show(fileName+" is downloaded!");
            }
        }
        static void searchInFiles(byte[] rawFile, string fileName, string keyword, string URL,System.Windows.Forms.ListBox L,string type)
        {
            string fileExtension = Path.GetExtension(fileName);
            ArrayList textExtensions = new ArrayList(new string[] { ".doc", ".docx", ".ppt", ".pptx", ".pdf", ".xls", ".xlsx" });
            if (textExtensions.Contains(fileExtension) && ("." + type == fileExtension))              //Checks if the given file type is a valid text document file type
            {
                TextExtractionResult text = new TextExtractor().Extract(rawFile);    //Using TikaOnDotNet to extract the data(text,Metadata,Content-Type) from any file
                if (text.Text.ToLower().Contains(keyword))
                {
                    hyperLinks.Add(URL);
                    L.Items.Add("[" + count + "]" + " " + hyperLinks[count] + " " + "\n");
                    count++;

                }
        }
        }
        static void showLinks()
        {
            
            int i = 0;
            foreach (string str in hyperLinks)
            {
                //Console.WriteLine((i++ + 1) + ". " + str);
                System.Windows.Forms.MessageBox.Show(str);
            }
                          //Gives 5 seconds to the user to input a link number(in which the buffer stream won't be lost) after finding a link
        }
        static void removeUnnecessaryLinks(ref ArrayList totalLinks)     //Removing the sublinks from the collection if the base link is present as they will duplicate when it'll crawl on the base link
        {
            totalLinks.Sort();         //O(n log n)
            totalLinks.Reverse();      //O(n)
            int count = 0;
            for (int j = 0; j < totalLinks.Count - 1; j++)
            {
                for (int i = j + 1; i < totalLinks.Count; i++)
                {
                    if (i == j && i < totalLinks.Count - 1)
                    {
                        i++;
                    }
                    if (j == totalLinks.Count - 1)
                    {
                        break;
                    }
                    if (totalLinks[j].ToString().Contains(totalLinks[i].ToString()))
                    {
                        totalLinks.RemoveAt(count);
                        i -= 2;
                    }
                }
                count++;
            }
            removeDuplicates(ref totalLinks, ref visited);
            removeDuplicates(ref totalLinks, ref toBeVisited);

        }
        static void removeDuplicates(ref ArrayList toCheck, ref ArrayList checkFrom)   //Comparing the totalLinks list with "visited" and "toBeVisited" lists and removing the duplicates
        {
            ArrayList temp = new ArrayList();

            foreach (string checkString in toCheck)              //O(mn)
            {
                foreach (string checkFromString in checkFrom)    //O(n) Linear Search
                {
                    if (checkFromString == checkString)
                    {
                        temp.Add(checkString);                   //temp is used because a link cannot be removed from the collection which is used in the loop as it will change its size and thus can give exception
                    }
                }
            }

            foreach (string str in temp)
            {
                toCheck.Remove(str);
            }
        }
    }

}

