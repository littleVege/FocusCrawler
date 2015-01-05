using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FocusCrawler
{
    class Program
    {
        //url template http://bj.esf.focus.cn/xiaoqu/6407/?From=comd_sp;
        private static Queue<string> _urlQueue = new Queue<string>();
        static void Main(string[] args)
        {

        }

        static void Download(string url)
        {
            var client = new WebClient();
            var html = "";
            var retry = 0;
            var success = false;
            do
            {
                try
                {
                    html = client.DownloadString(url);
                    success = true;
                }
                catch (WebException e)
                {
                    switch (e.Status)
                    {
                        case WebExceptionStatus.Timeout:
                        case WebExceptionStatus.SendFailure:
                        case WebExceptionStatus.ReceiveFailure:
                        case WebExceptionStatus.ConnectionClosed:
                            retry++;
                            break;
                        case WebExceptionStatus.ProtocolError:
                            success = true;
                            break;
                        default:
                            _urlQueue.Enqueue(url);
                            success = true;
                            break;
                    }
                    /*TODO:log this Error and time*/
                }
            } while (retry <= 3 || success);

        }

        static void ResolveItem(string url,string htmlStr)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlStr);
            var itemDic = new Dictionary<string, object>();
            var document = htmlDoc.DocumentNode;
            var cid = new Regex("\\d+",RegexOptions.ECMAScript).Match(url).ToString();
            var title = document.SelectSingleNode("//*[@class='f_yahei f26 p_name']").InnerText;
            var ranks = document.SelectNodes("//*[@class='f36 f_arial c136']");
            var assortRank = ranks[0].InnerText;
            var trifficRank = ranks[1].InnerText;
            var region = document.SelectSingleNode("//dl[@class='r_rank']/dt").InnerText;
            var price = document.SelectSingleNode("//b[@class='f_arial f24']").InnerText;
            var assortRate = document.SelectSingleNode("//i[@class='fb f18 f_arial']").InnerText;
            assortRate = new Regex("\\d+").Match(assortRate).ToString();
            itemDic.Add("cid",int.Parse(cid));
            itemDic.Add("url",url);
            itemDic.Add("title",title);
            itemDic.Add("assort_rank",int.Parse(assortRank));
            itemDic.Add("triffic_rank",int.Parse(trifficRank));
            itemDic.Add("region",region);
            itemDic.Add("price",double.Parse(price));
            itemDic.Add("assort_rate",double.Parse(assortRate)/100);

            var scripts = document.SelectNodes("//script");
            foreach (HtmlNode script in scripts)
            {
                var inner = script.InnerText;
                if (new Regex("parkAssort[\\s]*=[\\s]*([^;])+;",RegexOptions.ECMAScript).IsMatch(inner))
                {
                    
                }
            }
        }

        static List<Dictionary<string, object>> ParseAssorts(string script)
        {
            var results = new List<Dictionary<string, object>>();
            var matched = new Regex("parkAssort[\\s]*=[\\s]*([^;])+;", RegexOptions.ECMAScript).Matches(script);
            if (matched.Count < 0)
            {
                return null;
            }
            var jsonStr = matched[1].ToString();
            var parkAssort = JObject.Parse(jsonStr);
            var assorts = (JArray)parkAssort["assort_15m"];

            return results;
        }
    }
}
