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
            RevisionType revisionType = RevisionType.Insertions | RevisionType.Deletions;
            int rejectedChangesCount = RejectChanges(inputStream, revisionType);
            Console.WriteLine("Rejected {0} revisions for {1} made by all authors.", rejectedChangesCount, revisionType);
        }
        /// <summary>
        ///  Rejects the insert and delete revisions made by all authors.
        /// </summary>
        static int RejectChanges(Stream inputStream, RevisionType revisionType)
        {
            int rejectedChangesCount = 0;
            //Creates new Word document instance for Word processing
            using (WordDocument document = new WordDocument())
            {
                //Opens the Word document containing tracked changes
                document.Open(inputStream, FormatType.Docx);
                inputStream.Dispose();
                //Iterates all the revisions present in the Word document
                for (int i = document.Revisions.Count - 1; i >= 0; i--)
                {
                    //Validates the revision type of current revision to accepts/rejects it
                    if ((document.Revisions[i].RevisionType & revisionType) != 0)
                    {
                        //Rejects the current revision
                        document.Revisions[i].Reject();
                        rejectedChangesCount++;
                    }
                    //Resets i to last item, since rejecting one revision will impact all its related revisions and lead to change in Revsions
                    if (i > document.Revisions.Count - 1)
                        i = document.Revisions.Count;
                }
                //Saves the Word document as DOCX format
                using (Stream docStream = File.Create(Path.GetFullPath(@"../../../OutputAfterRejectingChanges.docx")))
                {
                    document.Save(docStream, FormatType.Docx);
                }
            }
            return rejectedChangesCount;
        }
    }
}
