using SFML.Graphics;
using ChessView.View.ResourcesManager;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine;
using ChessEngine.Maths;
using ChessEngine.Moves;

namespace ChessView.View
{
    public class ChessBoard2D: AObject2D
    {
        public static readonly float MODEL_2_VIEW_X = 84;
        public static readonly float MODEL_2_VIEW_Y = 83;
        public static readonly Vector2f CHESSBOARD2D_OFFSET = new Vector2f(42, 42);

        public static readonly TextureManager TextureManager;
        private static Dictionary<ChessAssetType, Texture> chessAssetTypeToTextures;

        private Dictionary<ChessPiece, ChessPiece2D> chessPieceToObject2D;

        private ChessBoard chessBoard;

        private float animationShiftDuration = 1;

        static ChessBoard2D()
        {
            TextureManager = new TextureManager();
            chessAssetTypeToTextures = new Dictionary<ChessAssetType, Texture>();

            RegisterChessAsset(ChessAssetType.CHESS_BOARD, @"Assets\Textures\chessBoard.png");

            RegisterChessAsset(ChessAssetType.CHESS_PIECE, @"Assets\Textures\chessPieces.png");
        }

        private static void RegisterChessAsset(ChessAssetType chessAssetType, string texturePath)
        {
            TextureManager.LoadTexture(texturePath);

            chessAssetTypeToTextures.Add(chessAssetType, TextureManager.GetTexture(texturePath));
        }

        public Vector2u ChessBoardSize
        {
            get
            {
                Texture chessBoardTexture = this.GetTextureFromChessAssetType(ChessAssetType.CHESS_BOARD);

                return chessBoardTexture.Size;
            }
        }

        public ChessBoard2D(ChessBoard chessBoard)
        {
            this.ObjectSprite = new Sprite(this.GetTextureFromChessAssetType(ChessAssetType.CHESS_BOARD));

            this.chessBoard = chessBoard;

            this.chessPieceToObject2D = new Dictionary<ChessPiece, ChessPiece2D>();

            this.chessBoard.NextTurnStarted += OnNextTurnStarted;
            this.chessBoard.PreviousTurnStarted += OnPreviousTurnStarted;

            this.chessBoard.MoveApplied += OnMoveApplied;
            this.chessBoard.MoveReverted += OnMoveReverted;
        }

        public void DrawIn(RenderWindow window, Time deltaTime)
        {
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();

            // TEST
            //this.Position = this.testAutoDriver.GetNextPosition(this.Position, deltaTime.AsSeconds());
            //

            base.DrawIn(window);

            foreach(IObject2D object2D in this.chessPieceToObject2D.Values)
            {
                object2D.DrawIn(window);
            }

            //this.entity2DManager.DrawIn(window, ref boundsView);

            //sw.Stop();

            //Console.WriteLine("time consume = " + sw.Elapsed);
        }

        public Texture GetTextureFromChessAssetType(ChessAssetType chessAssetType)
        {
            return chessAssetTypeToTextures[chessAssetType];
        }

        public Vector2f ConvertChessPositionTo2D(ChessPiecePosition chessPiecePosition)
        {
            return new Vector2f(
                chessPiecePosition.X * MODEL_2_VIEW_X + CHESSBOARD2D_OFFSET.X,
                chessPiecePosition.Y * MODEL_2_VIEW_Y + CHESSBOARD2D_OFFSET.Y);
        }

        public ChessPiecePosition Convert2DToChessPosition(Vector2f chessPiecePosition2D)
        {
            return new ChessPiecePosition(
                (int) ((chessPiecePosition2D.X - CHESSBOARD2D_OFFSET.X) / MODEL_2_VIEW_X),
                (int) ((chessPiecePosition2D.Y - CHESSBOARD2D_OFFSET.Y) / MODEL_2_VIEW_Y));
        }

        private void OnNextTurnStarted(ChessTurn nextChessTurn)
        {
            if (this.chessBoard.ChessTurns.Count == 1)
            {
                this.InitChessBoard2D();
            }
        }

        private void OnPreviousTurnStarted(ChessTurn previousChessTurn)
        {

        }

        private void OnMoveApplied(ChessPieceMovesContainer moveApplied)
        {
            this.UpdateChessBoard2D(moveApplied, false);
        }

        private void OnMoveReverted(ChessPieceMovesContainer moveReverted)
        {
            this.UpdateChessBoard2D(moveReverted, true);
        }

        private void InitChessBoard2D()
        {
            this.chessPieceToObject2D.Clear();

            foreach (ChessPiece chessPiece in this.chessBoard.ChessPiecesOnBoard)
            {
                this.chessPieceToObject2D.Add(chessPiece, new ChessPiece2D(this, chessPiece));
            }

            foreach (ChessPiece chessPiece in this.chessBoard.ChessPiecesCemetery)
            {
                this.chessPieceToObject2D.Add(chessPiece, new ChessPiece2D(this, chessPiece));
            }
        }

        private void UpdateChessBoard2D(ChessPieceMovesContainer moves, bool isRevertMove)
        {
            foreach (IChessPieceMove move in moves.ChessPieceMoves)
            {
                ChessPiece2D concernedChessPiece2D = this.chessPieceToObject2D[move.ConcernedChessPiece];

                if (move is ShiftChessPieceMove)
                {
                    ShiftChessPieceMove shiftChessPieceMove = move as ShiftChessPieceMove;
                    Vector2f newPosition2D = this.ConvertChessPositionTo2D(isRevertMove ? shiftChessPieceMove.FromPosition : shiftChessPieceMove.ToPosition);

                    concernedChessPiece2D.PlayPositionAnimation(this.animationShiftDuration, newPosition2D);
                }
                else if(move is KillChessPieceMove)
                {
                    KillChessPieceMove shiftChessPieceMove = move as KillChessPieceMove;

                    concernedChessPiece2D.PlayPositionAnimation(this.animationShiftDuration, new Vector2f(-100, -100));
                }
            }
        }

        public void Dispose(ChessBoard chessBoard)
        {
            
        }


    }
}
