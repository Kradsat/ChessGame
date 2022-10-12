using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, int tileCountY)
    {
        List<Vector2Int> r = new List<Vector2Int>();

        int direction = (team == 0) ? 1 : -1;

        // One in front
        if (board[currentX,currentY+direction] == null)
        {
            r.Add(new Vector2Int(currentX, currentY + direction));
        }

        // Two in front
        if (board[currentX, currentY + direction] == null)
        {
            //White team
            if (team == 0 && currentY == 1 && board[currentX, currentY + (direction * 2)] == null)
            {
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));
            }
            //Black team
            if (team == 1 && currentY == 6 && board[currentX, currentY + (direction * 2)] == null)
            {
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));
            }
        }

        // Capture
        if (currentX != tileCountX-1)
        {
            if (board[currentX + 1, currentY + direction] != null && board[currentX + 1, currentY + direction].team != team)
            {
                r.Add(new Vector2Int(currentX + 1, currentY + direction));
            }
        }

        if (currentX != 0)
        {
            if (board[currentX - 1, currentY + direction] != null && board[currentX - 1, currentY + direction].team != team)
            {
                r.Add(new Vector2Int(currentX - 1, currentY + direction));
            }
        }

        return r;
    }
    public override SpecialMove GetSpecialMove(ref ChessPiece[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> avalibleMoves)
    {
        int direction = (team == 0) ? 1 : -1;
        if (team == 0 && currentY == 6 || team == 1 && currentY == 1)
        {
            return SpecialMove.Promotion;
        }

        // EnPassant
        if (moveList.Count>0)
        {
            Vector2Int[] lasMove = moveList[moveList.Count - 1];
            if (board[lasMove[1].x, lasMove[1].y].type == ChessPieceType.Pawn)//if the last piece moved was a pawn
            {
                if (Mathf.Abs(lasMove[0].y - lasMove[1].y) == 2)// if the last move was a +2 move in either dirrection 
                {
                    if (board[lasMove[1].x, lasMove[1].y].team != team) // if the move was from the other team
                    {
                        if (lasMove[1].y == currentY) // if both pawns are at the same Y
                        {
                            if (lasMove[1].x == currentX + 1) // landed right
                            {
                                avalibleMoves.Add(new Vector2Int(currentX + 1, currentY + direction));
                                return SpecialMove.EnPassant;
                            }
                            if (lasMove[1].x == currentX - 1) // landed left
                            {
                                avalibleMoves.Add(new Vector2Int(currentX - 1, currentY + direction));
                                return SpecialMove.EnPassant;
                            }
                        }
                    }
                }
            }
        }

        return SpecialMove.None;
    }
}
