using HashFunction;
using HeuristicCalculator;

namespace AI
{
    public class QuartoSearchTree
    {
        public const int MAXGAMEBOARD = 16;
        public const int NULLPIECE = 55;

        public class Node
        {
            public string[] gameBoard = new string[MAXGAMEBOARD];
            public Node[] children = new Node[MAXGAMEBOARD];
            public Node parent;
            public Piece[] pieces = new Piece[MAXGAMEBOARD];
            public int pieceToPlay;
            public int moveOnBoard;
        }

        public Node root;
        public QuartoSearchTree()
        {
            root = null;
        }

        public moveData generateTree(string[] newGameBoard, int piece, Piece[] currentPieces, int difficulty)
        {
            int piecesOnBoard;
            int maxDepth;
            int positionToBlock;
            bool boardBlockable = false;
            string moveToSend;
            string pieceOnDeck;

            Node newNode = new Node();
            newNode.gameBoard = newGameBoard;
            newNode.pieceToPlay = piece;
            root = newNode;
            Node currentNode = root;
            Node parentNode;
            parentNode = currentNode;
            currentNode.pieces = currentPieces;

            positionToBlock = AIFunctions.findWinningPositionToBlock(newGameBoard, currentPieces[piece].piece);
            if (difficulty == 3 && positionToBlock != -1 && Heuristic.calculateHeuristic(newGameBoard, currentPieces[piece].piece) > 0)
            {
                boardBlockable = true;
            }

            // Sets tree depth according to how many pieces are on the board
            piecesOnBoard = AIFunctions.countPiecesOnBoard(newGameBoard);
            maxDepth = AIFunctions.setTreeDepth(piecesOnBoard, difficulty);

            // Generates game tree
            generateChildrenGamestate(currentNode, parentNode, piece, maxDepth, 0);

            // Finds the best move in the game tree based on a heuristic.
            winningMove move = NegaMax.searchForBestPlay(currentNode, maxDepth, 0, -MAXGAMEBOARD, MAXGAMEBOARD, true);

            if (boardBlockable && !move.isWin && piecesOnBoard != MAXGAMEBOARD - 1)
            {
                // Erases old winning move then adds new move to board.
                // Also sets old piece to playable again.
                move.winningNode.gameBoard[move.winningNode.moveOnBoard] = null;
                move.winningNode.gameBoard[positionToBlock] = currentPieces[piece].piece;
                move.winningNode.pieces[piece].setPlayable(true);

                move.winningNode = AIFunctions.checkForOpponentWin(move.winningNode, currentPieces[piece].piece);
                
                pieceOnDeck = move.winningNode.pieceToPlay == NULLPIECE ?
                    NULLPIECE.ToString() : move.winningNode.pieces[move.winningNode.pieceToPlay].piece;

                moveToSend = move.winningNode.pieces[positionToBlock].piece;
            }
            else
            {
                if (piecesOnBoard != 0 && move.winningNode.pieceToPlay != NULLPIECE)
                    move.winningNode = AIFunctions.checkForOpponentWin(move.winningNode, null);

                moveToSend = move.winningNode.pieces[move.winningNode.moveOnBoard].piece;
                //Checks for win by opponent, given the piece chosen
                //If win it makes it equal to the next child and so on
                pieceOnDeck = move.winningNode.pieceToPlay == NULLPIECE ?
                    NULLPIECE.ToString() : move.winningNode.pieces[move.winningNode.pieceToPlay].piece;
            }

            return new moveData
            {
                lastMoveOnBoard = moveToSend,
                pieceToPlay = pieceOnDeck
            };
        }

        // Generates all the moves that could possibly be made for a given gamestate and piece.
        public static void generateChildrenGamestate(Node currentNode, Node parentNode, int piece, int maxDepth, int depth)
        {
            int boardPosCount = 0;
            int childCount = 0;

            while (boardPosCount < MAXGAMEBOARD)
            {
                if (currentNode.gameBoard[boardPosCount] == null)
                {
                    Node nextNode = new Node();
                    for (int counter = 0; counter < MAXGAMEBOARD; counter++)
                    {
                        string value = currentNode.gameBoard[counter];
                        nextNode.gameBoard[counter] = value;
                    }
                    nextNode.gameBoard[boardPosCount] = currentNode.pieces[piece].getPiece();

                    nextNode.moveOnBoard = boardPosCount;

                    nextNode.parent = parentNode;
                    parentNode.children[childCount] = nextNode;

                    AIFunctions.copyPieceMap(nextNode, currentNode, piece);
                    nextNode.pieceToPlay = NULLPIECE;

                    childCount++;
                    if (depth < maxDepth)
                    {
                        Node childNode = nextNode;
                        Node nextParentNode = childNode;
                        int newDepth = depth + 1;
                        generateChildrenPiece(childNode, nextParentNode, maxDepth, newDepth);
                    }
                }
                boardPosCount++;
            }
        }

        // Generates all the piece selections that could possibly be made for a given gamestate.
        public static void generateChildrenPiece(Node currentNode, Node parentNode, int maxDepth, int depth)
        {
            int pieceMapCount = 0;
            int childCount = 0;

            while (pieceMapCount < MAXGAMEBOARD)
            {
                if (currentNode.pieces[pieceMapCount].getPlayablePiece())
                {
                    Node nextNode = new Node();

                    for (int counter = 0; counter < MAXGAMEBOARD; counter++)
                    {
                        string value = currentNode.gameBoard[counter];
                        nextNode.gameBoard[counter] = value;
                    }

                    nextNode.pieceToPlay = pieceMapCount;
                    int newMove = currentNode.moveOnBoard;

                    nextNode.moveOnBoard = newMove;
                    nextNode.parent = parentNode;

                    parentNode.children[childCount] = nextNode;

                    AIFunctions.copyPieceMap(nextNode, currentNode, pieceMapCount);

                    childCount++;
                    if (depth < maxDepth)
                    {
                        Node childNode = nextNode;
                        Node nextParentNode = childNode;
                        int piece = nextNode.pieceToPlay;
                        int newDepth = depth + 1;
                        generateChildrenGamestate(childNode, nextParentNode, piece, maxDepth, newDepth);
                    }
                }
                pieceMapCount++;
            }
        }

        static void Main(string[] args)
        {

            QuartoSearchTree tree = new QuartoSearchTree();
            string[] board = { "B3", "B4", "D1", "C2",
                               "D4", "A2", "A1", "B2",
                               null, "B1", "A3", null,
                               "C3", "D2", "A4", null
                             };
            Piece[] pieces = new Piece[MAXGAMEBOARD];
            moveData move = new moveData();
            pieces[0].setValues("A1", false);
            pieces[1].setValues("A2", false);
            pieces[2].setValues("A3", false);
            pieces[3].setValues("A4", false);
            pieces[4].setValues("B1", false);
            pieces[5].setValues("B2", false);
            pieces[6].setValues("B3", false);
            pieces[7].setValues("B4", false);
            pieces[8].setValues("C1", true);
            pieces[9].setValues("C2", false);
            pieces[10].setValues("C3",false);
            pieces[11].setValues("C4",true);
            pieces[12].setValues("D1",false);
            pieces[13].setValues("D2",false);
            pieces[14].setValues("D3",true);
            pieces[15].setValues("D4", false);

            move = tree.generateTree(board, 8, pieces, 3);
        }
    }
    public struct Piece
        {
            public string piece;
            public bool playable;

            public void setValues(string piece, bool playable)
            {
                this.piece = piece;
                this.playable = playable;
            }

            public void setPlayable(bool playable)
            {
                this.playable = playable;
            }
            public bool getPlayablePiece()
            {
                return this.playable;
            }

            public string getPiece()
            {
                return this.piece;
            }
        }

        public struct moveData
        {
            public string lastMoveOnBoard;
            public string pieceToPlay;
        }
}