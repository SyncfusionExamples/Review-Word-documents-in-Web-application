using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Reads the input Word document
            Stream inputStream = File.OpenRead(Path.GetFullPath(@"../../../../../../Data/InputWithTrackedChanges.docx"));
            string authorName = "Steven Buchanan";
            int acceptedChangesCount = AcceptChanges(inputStream, authorName);
            Console.WriteLine("Accepted {0} changes made by {1}.", acceptedChangesCount, authorName);
        }
        /// <summary>
        ///  Accepts all the changes made by an author.
        /// </summary>
        static int AcceptChanges(Stream inputStream, string authorName)
        {
            int acceptedChangesCount = 0;
            //Creates new Word document instance for Word processing
            using (WordDocument document = new WordDocument())
            {
                //Opens the Word document containing tracked changes
                document.Open(inputStream, FormatType.Docx);
                inputStream.Dispose();
                //Iterates all the revisions present in the Word document
                for (int i = document.Revisions.Count - 1; i >= 0; i--)
                {
                    //Validates the author of current revision to accepts/rejects it
                    if (document.Revisions[i].Author == authorName)
                    {
                        //Accepts the current revision
                        document.Revisions[i].Accept();
                        acceptedChangesCount++;
                    }
                    //Resets i to last item, since accepting one revision will impact all its related revisions and leads to change in Revsions
                    if (i > document.Revisions.Count - 1)
                        i = document.Revisions.Count;
                }
                //Saves the Word document as DOCX format
                using (Stream docStream = File.Create(Path.GetFullPath(@"../../../OutputAfterAcceptingChanges.docx")))
                {
                    document.Save(docStream, FormatType.Docx);
                }
            }
            return acceptedChangesCount;
        }
    }
}
