using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameSerializer
{
    string GetSaveLocationBinary(string fileName)
    {
        string folder = Application.persistentDataPath;
        string file = fileName;
        string fullPath = Path.Combine(folder, file);
        return fullPath;
    }

    public Board LoadGameBinary(string fileName, int gameMode)
    {
        string filePath = GetSaveLocationBinary(fileName);
        if (File.Exists(filePath))
        {
            using (Stream s = File.OpenRead(filePath))
            {
                using (BinaryReader r = new BinaryReader(s))
                {
                    //READ THE HEAD
                    string head = new string(r.ReadChars(4));
                    
                    if (!head.Equals("BASE"))
                    {
                        Debug.Log("NEW GAME");
                        GameManager.Instance.Restart();
                        return null;
                    }
                    int highscore = r.ReadInt32();
                    int numRecords = r.ReadInt32();

                    Board board = new Board();
                    board.pieces = new List<Piece>();
                    //READ BoDY
                    for (int i = 0; i < numRecords; i++)
                    {
                        int pieceScore = r.ReadInt32();
                        float x = r.ReadSingle();
                        float y = r.ReadSingle();

                        Piece piece = new Piece();
                        piece.score = pieceScore;
                        piece.position = new Vector2(x, y);

                        board.pieces.Add(piece);

                    }
                    //Return highscore
                    board.highscore = highscore;
                   
                    return board;
                }
            }
        }
        else
            return new Board();

       
    }


    public void SaveGameBinary(Board board, int gameMode)
    {
       
        //  Filetype (4*char)
        //  Number of items (integer)
        //
      


        string filePath = GetSaveLocationBinary("threesixty"+gameMode+".dat");


        if (File.Exists(filePath))
        {
            File.Delete(filePath);
           
            Debug.Log("DELETED");
        }

        
        using (Stream s = File.OpenWrite(filePath))
        {
            using (BinaryWriter w = new BinaryWriter(s))
            {
                //Write out a header  (8 bytes header)
                // highscore aswell
                w.Write("BASE".ToCharArray());
                w.Write(board.highscore);
                w.Write(board.pieces.Count);
                

                //Body  (each record is 12 bytes long)
                //  Score (integer) 4
                //  spot    (integer)  4
                //  index   (integer)  4
                foreach (Piece p in board.pieces)
                {
                    w.Write(p.score);
                    w.Write(p.position.x);
                    w.Write(p.position.y);
                }
            }
        }

       
    }

    public void CreateNewGame(int gameMode)
    {
        string filePath = GetSaveLocationBinary("threesixty"+gameMode+".dat");


        if (File.Exists(filePath))
        {
            File.Delete(filePath);

            Debug.Log("DELETED");
        }

    }


    //public void TutorialGameBinary(Board board)
    //{

    //    //  Filetype (4*char)
    //    //  Number of items (integer)
    //    //



    //    string filePath = GetSaveLocationBinary("turoialgame.dat");


    //    if (File.Exists(filePath))
    //    {
    //        File.Delete(filePath);

    //        Debug.Log("TUTORIAL");
    //    }


    //    using (Stream s = File.OpenWrite(filePath))
    //    {
    //        using (BinaryWriter w = new BinaryWriter(s))
    //        {
    //            //Write out a header  (8 bytes header)
    //            // highscore aswell
    //            w.Write("BASE".ToCharArray());
    //            w.Write(board.highscore);
    //            w.Write(board.pieces.Count);


    //            //Body  (each record is 12 bytes long)
    //            //  Score (integer) 4
    //            //  spot    (integer)  4
    //            //  index   (integer)  4
    //            foreach (Piece p in board.pieces)
    //            {
    //                w.Write(p.score);
    //                w.Write(p.position.x);
    //                w.Write(p.position.y);
    //            }
    //        }
    //    }


    //}
}