using Godot;
using System;
using System.Collections.Generic;
using static Godot.GD;

public partial class NewScript : Node
{
    static int x_size = 4;
    static int y_size = 3;
    //2D Data Structure/Matrix
    //public List<List<int>> matrix = new List<List<int>>();
    public int[,] matrix = new int[x_size,y_size]   ;

    //only 1 matrix in use but a list is used, so that differently sized arrays can be used.
    //public List<int[,]> matrix = new List<int[,]>();

    //1D Projection of the Data Structure, int in matrix corresponds to the index in these lists
    public List<int> x_position = new List<int>();
    public List<int> y_position = new List<int>();
    public List<string> node_type = new List<string>(); 



    private void RestartGenerationCycle(int x_size, int y_size)
    {
        x_position.Clear();
        y_position.Clear();
        node_type.Clear();
        //matrix.Clear();

        //matrix.Add(int[x_size,y_size]);

    }


}
