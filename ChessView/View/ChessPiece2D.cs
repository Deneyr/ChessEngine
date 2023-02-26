using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChessEngine;
using SFML.Graphics;
using SFML.System;

namespace ChessView.View
{
    public class ChessPiece2D : AObject2D
    {
        public ChessPiece ChessPiece
        {
            get;
            private set;
        }

        public ChessPiece2D(ChessBoard2D chessBoard2D, ChessPiece chessPiece)
        {
            this.ChessPiece = chessPiece;

            Texture chessPiecesTexture = chessBoard2D.GetTextureFromChessAssetType(ChessAssetType.CHESS_PIECE);

            this.ObjectSprite = new Sprite(chessPiecesTexture, this.GetTextureRectFrom(chessPiecesTexture, this.ChessPiece));
            this.ObjectSprite.Origin = new Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);

            this.Position = chessBoard2D.GetChessPiece2DPositionFrom(this.ChessPiece.ChessPiecePosition);
        }

        public void UpdateChessPiece(ChessBoard2D chessBoard2D)
        {
            Texture chessPiecesTexture = chessBoard2D.GetTextureFromChessAssetType(ChessAssetType.CHESS_PIECE);
            this.ObjectSprite = new Sprite(chessPiecesTexture, this.GetTextureRectFrom(chessPiecesTexture, this.ChessPiece));

            this.ObjectSprite.Origin = new Vector2f(this.ObjectSprite.TextureRect.Width / 2, this.ObjectSprite.TextureRect.Height / 2);
        }

        private IntRect GetTextureRectFrom(Texture chessPiecesTexture, ChessPiece chessPiece)
        {
            Vector2i textureCoord = this.GetTextureCoordFrom(chessPiece.ChessPieceType);

            if(chessPiece.Owner.YDirection > 0)
            {
                textureCoord.Y = 1;
            }

            int textureWidth = (int)(chessPiecesTexture.Size.X / 6);
            int textureHeight = (int)(chessPiecesTexture.Size.Y / 2);

            Vector2i texturePixelCoord = new Vector2i(textureCoord.X * textureWidth, textureCoord.Y * textureHeight);

            return new IntRect(texturePixelCoord, new Vector2i(textureWidth, textureHeight));
        }

        private Vector2i GetTextureCoordFrom(ChessPieceType chessPieceType)
        {
            switch (chessPieceType)
            {
                case ChessPieceType.PAWN:
                    return new Vector2i(0, 0);
                case ChessPieceType.KNIGHT:
                    return new Vector2i(1, 0);
                case ChessPieceType.BISHOP:
                    return new Vector2i(2, 0);
                case ChessPieceType.ROOK:
                    return new Vector2i(3, 0);
                case ChessPieceType.QUEEN:
                    return new Vector2i(4, 0);
                case ChessPieceType.KING:
                    return new Vector2i(5, 0);
            }
            return new Vector2i();
        }
    }
}
