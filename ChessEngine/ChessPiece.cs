using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine.ChessModels;
using ChessEngine.ChessModels.Monitors;
using ChessEngine.Maths;
using ChessEngine.Moves;
using ChessEngine.Players;

namespace ChessEngine
{
    public class ChessPiece : ChessPieceTemplate
    {
        public ChessPieceTemplate ChessPieceTemplate
        {
            get;
            internal set;
        }

        public IPlayer Owner
        {
            get;
            private set;
        }

        public ChessPiecePosition ChessPiecePosition
        {
            get;
            internal set;
        }

        public bool HasAlreadyMoved
        {
            get;
            internal set;
        }

        public override ChessPieceType ChessPieceType
        {
            get
            {
                return this.ChessPieceTemplate.ChessPieceType;
            }
        }

        internal override ActionChessModelMonitor ActionModelMonitor
        {
            get
            {
                return this.ChessPieceTemplate.ActionModelMonitor;
            }
        }

        internal override ReactionChessModelMonitor ReactionModelMonitor
        {
            get
            {
                return this.ChessPieceTemplate.ReactionModelMonitor;
            }
        }

        public bool IsAlive
        {
            get
            {
                return this.ChessPiecePosition.X >= 0 && this.ChessPiecePosition.Y >= 0;
            }
        }

        public ChessPiece(ChessPieceTemplate chessPieceTemplate, IPlayer owner, ChessPiecePosition startPosition)
        {
            this.ChessPieceTemplate = chessPieceTemplate;

            this.Owner = owner;

            this.ChessPiecePosition = startPosition;
            this.HasAlreadyMoved = false;
        }

        public bool CreateMove(ChessBoard chessBoard, IChessMoveInfluence chessMoveInfluence, out ChessPieceMovesContainer chessPieceMove)
        {
            chessPieceMove = null;

            if (chessMoveInfluence != null)
            {
                bool result = chessMoveInfluence.CreateMove(chessBoard, this, out chessPieceMove);

                if (result)
                {
                    result = this.ReactionModelMonitor.Visit(chessBoard, chessPieceMove);
                }

                if (result)
                {
                    result = chessBoard.IsGivenMovesGetChecked(chessPieceMove);
                }

                if(result == false)
                {
                    chessPieceMove = null;
                }

                return result;
            }
            return false;
        }

        //public bool IsMoveAllowed(ChessBoard chessBoard, IChessMoveInfluence chessMoveInfluence)
        //{
        //    if (chessMoveInfluence != null)
        //    {
        //        return chessMoveInfluence.IsMoveAllowed(chessBoard, this) && chessBoard.IsGivenMovesGetChecked(chessMoveInfluence);
        //    }
        //    return this.CreateMove;
        //}

        internal List<ChessPieceMovesContainer> GetAllPossibleMovesWithoutCheckRestriction(ChessBoard chessBoard)
        {
            List<ChessPieceMovesContainer> possibleMoves = this.ActionModelMonitor.GetAllPossibleMoves(chessBoard, this);
            return possibleMoves.Where(pElem => this.ReactionModelMonitor.Visit(chessBoard, pElem)).ToList();
        }

        public List<ChessPieceMovesContainer> GetAllPossibleMoves(ChessBoard chessBoard)
        {
            List<ChessPieceMovesContainer> possibleMoves = this.GetAllPossibleMovesWithoutCheckRestriction(chessBoard);
            return possibleMoves.Where(pElem => chessBoard.IsGivenMovesGetChecked(pElem)).ToList();
        }
    }
}
