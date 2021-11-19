using System.Collections.Generic;

// https://en.wikipedia.org/wiki/Chess_symbols_in_Unicode
public enum Figures
{
    WhiteChessKing = 0,
    WhiteChessQueen,
    WhiteChessRook,
    WhiteChessBishop,
    WhiteChessKnight,
    WhiteChessPawn,
    BlackChessKing,
    BlackChessQueen,
    BlackChessRook,
    BlackChessBishop,
    BlackChessKnight,
    BlackChessPawn
}

static class ChessFigures
{
    public static Dictionary<int, string> Codes = new Dictionary<int, string>()
    {
        {0, "\u2654"}, // WhiteChessKing
        {1, "\u2655"},
        {2, "\u2656"},
        {3, "\u2657"},
        {4, "\u2658"},
        {5, "\u2659"},
        {6, "\u265A"},
        {7, "\u265B"},
        {8, "\u265C"},
        {9, "\u265D"},
        {10, "\u265E"},
        {11, "\u265F"}
    };

    public static HashSet<string> WhiteFigures = new HashSet<string>()
    {
        "\u2654",
        "\u2655",
        "\u2656",
        "\u2657",
        "\u2658",
        "\u2659"
    };

    public static HashSet<string> BlackFigures = new HashSet<string>()
    {
        "\u265A",
        "\u265B",
        "\u265C",
        "\u265D",
        "\u265E",
        "\u265F"
    };
}