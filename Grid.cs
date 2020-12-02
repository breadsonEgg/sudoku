using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// create 9x9 grid
// create list of numbers 1-9
// find an empty cell and assign a random number from the list to it
// check if number has not already been used on the row
// check if the number has not already been used on the column
// check if number has not already been used within the 3x3 subgrid
// remove numbers till only 17 numbers are left on the grid
// select random cell that has a number on it and remove it
// save/remember the number removed from the cell? 

public class Grid : MonoBehaviour
{
    public int columns = 9;
    public int rows = 9;
    public float every_square_offset = 0.0f;

    public GameObject grid_square;

    public Vector2 start_position = new Vector2(0.0f, 0.0f);

    public float square_scale = 1.0f;

    private List<GameObject> grid_squares_ = new List<GameObject>();
    private int selected_grid_data = -1;

    void Start()
    {

        if (grid_square.GetComponent<GridSquare>() == null)
            Debug.LogError("grid_square object need to have GridSquare script attached");
        CreateGrid();
        SetGridNumbers("Easy");


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateGrid()
    {
        SpawnGridSquares();
        SetSquaresPosition();
    }

    private void SpawnGridSquares()
    {
        //0, 1,3,4,5,6
        //7,8,9,10 ...
        int square_index = 0;

        for(int row =0; row < rows; row++)
        {
            for(int column =0; column < columns; column++)
            {
                grid_squares_.Add(Instantiate(grid_square) as GameObject);
                grid_squares_[grid_squares_.Count - 1].GetComponent<GridSquare>().SetSquareIndex(square_index);
                grid_squares_[grid_squares_.Count - 1].transform.parent = this.transform; //instantiate this game object as a child of the object holding this script
                grid_squares_[grid_squares_.Count - 1].transform.localScale = new Vector3(square_scale, square_scale, square_scale);

                square_index++;

            }
        }
    }

    private void SetSquaresPosition()
    {
        var square_rect = grid_squares_[0].GetComponent<RectTransform>();
        Vector2 offset = new Vector2();
        offset.x = square_rect.rect.width * square_rect.transform.localScale.x + every_square_offset;
        offset.y = square_rect.rect.height * square_rect.transform.localScale.y + every_square_offset;

        int column_number = 0;
        int row_number = 0;

        foreach (GameObject square in grid_squares_)
        {

            if(column_number + 1 > columns)
            {
                row_number++;
                column_number = 0;
            }

            var pos_x_offset = offset.x * column_number;
            var pos_y_offset = offset.y * row_number;
            square.GetComponent<RectTransform>().anchoredPosition = new Vector3(start_position.x + pos_x_offset, start_position.y - pos_y_offset);
            column_number++;
        }
    }

    private void SetGridNumbers(string level)
    {
        selected_grid_data = Random.Range(0, SudokuData.Instance.sudoku_game[level].Count);
        var data = SudokuData.Instance.sudoku_game[level][selected_grid_data];

        setGridSquareData(data);
     
        
        
        // foreach (var square in grid_squares_)
      // {
      //      square.GetComponent<GridSquare>().SetNumber(Random.Range(0, 10));
      // }
    }

    private void setGridSquareData(SudokuData.SudokuBoardData data)
    {
        for(int index = 0; index < grid_squares_.Count; index++)
        {
            grid_squares_[index].GetComponent<GridSquare>().SetNumber(data.unsolved_data[index]);
            grid_squares_[index].GetComponent<GridSquare>().SetCorrectNumber(data.solved_data[index]);
        }
    }
}
