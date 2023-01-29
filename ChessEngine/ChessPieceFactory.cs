using ChessEngine.ChessModels;
using ChessEngine.Maths;
using ChessEngine.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessEngine
{
    internal class ChessPieceFactory
    {
        private Dictionary<ChessPieceType, ChessPieceTemplate> chessPieceLibrary;

        public ChessPieceFactory()
        {
            this.chessPieceLibrary = new Dictionary<ChessPieceType, ChessPieceTemplate>();

            ChessPieceTemplate chessPieceTemplate;

            // Bishop template
            chessPieceTemplate = new ChessPieceTemplate();
            chessPieceTemplate.ChessPieceType = ChessPieceType.BISHOP;
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(1, 1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-1, -1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-1, 1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(1, -1, int.MaxValue));
            chessPieceTemplate.ReactionModelMonitor.AddReactionChessModel(new CaptureReactionChessModel());
            this.chessPieceLibrary.Add(chessPieceTemplate.ChessPieceType, chessPieceTemplate);

            // Rook template
            chessPieceTemplate = new ChessPieceTemplate();
            chessPieceTemplate.ChessPieceType = ChessPieceType.ROOK;
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(0, 1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(0, -1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-1, 0, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(1, 0, int.MaxValue));
            chessPieceTemplate.ReactionModelMonitor.AddReactionChessModel(new CaptureReactionChessModel());
            this.chessPieceLibrary.Add(chessPieceTemplate.ChessPieceType, chessPieceTemplate);

            // Queen template
            chessPieceTemplate = new ChessPieceTemplate();
            chessPieceTemplate.ChessPieceType = ChessPieceType.QUEEN;
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(1, 1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-1, -1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-1, 1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(1, -1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(0, 1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(0, -1, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-1, 0, int.MaxValue));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(1, 0, int.MaxValue));
            chessPieceTemplate.ReactionModelMonitor.AddReactionChessModel(new CaptureReactionChessModel());
            this.chessPieceLibrary.Add(chessPieceTemplate.ChessPieceType, chessPieceTemplate);

            // Knight template
            chessPieceTemplate = new ChessPieceTemplate();
            chessPieceTemplate.ChessPieceType = ChessPieceType.KNIGHT;
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(2, -1, 1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(2, 1, 1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-2, -1, 1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-2, 1, 1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-1, 2, 1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(1, 2, 1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(-1, -2, 1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new ShiftActionChessModel(1, -2, 1));
            chessPieceTemplate.ReactionModelMonitor.AddReactionChessModel(new CaptureReactionChessModel());
            this.chessPieceLibrary.Add(chessPieceTemplate.ChessPieceType, chessPieceTemplate);

            // Pawn template
            chessPieceTemplate = new ChessPieceTemplate();
            chessPieceTemplate.ChessPieceType = ChessPieceType.PAWN;
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new PawnShiftActionChessModel());
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new PawnTakeShiftActionChessModel(-1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new PawnTakeShiftActionChessModel(1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new EnPassantShiftActionChessModel(-1));
            chessPieceTemplate.ActionModelMonitor.ShiftActionChessModels.AddShiftActionChessModel(new EnPassantShiftActionChessModel(1));
            chessPieceTemplate.ActionModelMonitor.PromoteActionChessModels.AddPromoteActionChessModel(new PromoteActionChessModel());
            chessPieceTemplate.ReactionModelMonitor.AddReactionChessModel(new CaptureReactionChessModel());
            chessPieceTemplate.ReactionModelMonitor.AddReactionChessModel(new EnPassantCaptureReactionChessModel());
            this.chessPieceLibrary.Add(chessPieceTemplate.ChessPieceType, chessPieceTemplate);
        }

        public ChessPiece CreateChessPiece(IPlayer owner, ChessPieceType chessPieceType, ChessPiecePosition chessPiecePosition)
        {
            ChessPieceTemplate chessPieceTemplate = this.chessPieceLibrary[chessPieceType];

            return new ChessPiece(chessPieceTemplate, owner, chessPiecePosition);
        }

        public void TransmuteChessPiece(ChessPieceType chessPieceType, ChessPiece chessPiece)
        {
            ChessPieceTemplate chessPieceTemplate = this.chessPieceLibrary[chessPieceType];

            chessPiece.ChessPieceTemplate = chessPieceTemplate;
        }

    }
}
