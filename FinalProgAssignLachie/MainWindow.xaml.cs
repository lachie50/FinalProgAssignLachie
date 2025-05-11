using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace testerrrr
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Movie[] movieArray;

        public MainWindow()
        {
            InitializeComponent();


            //movie sample data
            Movie movie1 = new Movie("m100", "Ice Age 2", "Carlos Saldanha", "Action", 2006, true);
            Movie movie2 = new Movie("m110", "Shrek 2", "Andrew Adamson", "Family", 2004, true);
            Movie movie3 = new Movie("m120", "Fast and Furious", "Vin Diesel", "Action", 2001, true);
            Movie movie4 = new Movie("m130", "The Silver Horizon", "Ava Thompson", "Sci-Fi", 2012, true);
            Movie movie5 = new Movie("m140", "Echoes of the Fallen", "Marcus Chen", "Action", 2015, true);


            //create new hashtable
            MovieHash movieDatabase = new MovieHash();
            //insert data into hash table
            movieDatabase.Add(movie1.MovieID, movie1);
            movieDatabase.Add(movie2.MovieID, movie2);
            movieDatabase.Add(movie3.MovieID, movie3);
            movieDatabase.Add(movie4.MovieID, movie4);
            movieDatabase.Add(movie5.MovieID, movie5);





            //getting value with key
            Movie request = movieDatabase.GetValue("m100");


            //populating and creating linked list
            LinkedList movieList = new LinkedList();
            movieList.Add(movie1);
            movieList.Add(movie2);
            movieList.Add(movie3);
            movieList.Add(movie4);
            movieList.Add(movie5);


            movieArray = new Movie[] { movie1, movie2, movie3, movie4, movie5, };

        }

        public class Movie
        {
            //defining properties
            public string MovieID { get; set; }
            public string Title { get; set; }
            public string Director { get; set; }
            public string Genre { get; set; }
            public int ReleaseYear { get; set; }
            public bool Availability { get; set; }
            public string CurrentBorrower { get; set; }
            public Queue<string> Waitlist { get; set; } = new Queue<string>();
            public bool IsBorrowed { get; set; } = false;

            public Movie() { }

            //initialise properties using constructor
            public Movie(string movieid, string title, string director, string genre, int releaseyear, bool availability)
            {
                MovieID = movieid;
                Title = title;
                Director = director;
                Genre = genre;
                ReleaseYear = releaseyear;
                Availability = availability;
            }
        }

        //hashtable class to store movies
        public class MovieHash
        {
            public int size = 5;  //size of hashtable
            public string[] keys;  //array to store movie ids
            public Movie[] values;  //array to store movie values

            //constructor to initialize arrays
            public MovieHash()
            {
                //storing keys and values
                keys = new string[size];
                values = new Movie[size];

            }

            //private method to generate a hash index based on the MovieID
            private int Hash(string key)
            {
                //set default value of hash to 0
                int hash = 0;

                //foreach loop gets ASCII value for each character in key
                foreach (char c in key)
                {
                    //adds value to hash
                    hash += c;
                }
                return hash % size;  //ensure index is within size of the array
            }

            //method to add a new movie into hashtable
            public void Add(string key, Movie movie)
            {
                int index = Hash(key);  //get initial index

                //if current slot is taken and not matching key, move to the next slot
                while (keys[index] != key && keys[index] != null)
                {
                    //move to next slot
                    index = (index + 1) % size;
                }

                keys[index] = key;
                values[index] = movie;
            }

            //method to get a movie by MovieID
            public Movie GetValue(string key)
            {
                int index = 0;  //get initial index
                int firstIndex = index;  //save original index to detect loop

                // search for the key using linear probing
                while (keys[index] != null)
                {
                    if (keys[index] == key)
                    {
                        return values[index];  //key found, return the movie
                    }
                    index = (index + 1) % size;  //move to the next index
                    if (index == firstIndex)
                    {
                        break;  //full loop completed key not found
                    }
                }
                return null;  //key not found return null
            }

        }

        //private class representing a node in the linked list
        public class Node
        {
            public Movie Data; //movie stored in this node
            public Node Next; //reference the next node

            //constructor to initialize the node with movie data
            public Node(Movie data)
            {
                Data = data;
                Next = null;
            }

        }

        //linked list to store movies
        public class LinkedList
        {
            private Node head;

            public void Add(Movie data)
            {
                //create a new node with the movie data
                Node newNode = new Node(data);

                //if the list is empty, set the new node as the head
                if (head == null)
                {
                    head = newNode;
                }
                else
                {
                    //else find the last node in the list
                    Node current = head;
                    while (current.Next != null)
                    {
                        current = current.Next;
                    }

                    //append the new node at the end
                    current.Next = newNode;
                }

            }

            //public void PrintList()
            //{
            //    Node current = head;
            //    while (current != null)
            //    {
            //        Console.WriteLine(current.Data + " -> ");
            //        current = current.Next;
            //    }
            //    Console.WriteLine("null");
            //}

        }

        //queue implementation
        public class Queue
        {
            private Movie[] elements; //array to store queued movies
            private int size; //current number of movies in the queue

            //constructor initialised the queue with a fixed capacity
            public Queue(int capacity)
            {
                elements = new Movie[capacity]; //allocate memory for the array 
                size = 0; //initially the queue is empty
            }

            //adds a movie to the end of the queue
            public void Enqueue(Movie item)
            {
                //if the queue is full, throw an error
                if (size == elements.Length)
                    throw new InvalidOperationException("Queue is full");

                elements[size] = item; //add the new movie at the end of the queue
                size++; //increase the size counter
            }

            //removes and returns the movie at the front of the queue
            public Movie Dequeue()
            {
                //if queue is empty theres nothing to dequeue so it throws an error
                if (IsEmpty())
                    throw new InvalidOperationException("Queue is empty");

                //store the first movie to return later
                Movie item = elements[0];

                //shift all remaining elements to the left one position
                for (int i = 1; i < size; i++)
                {
                    elements[i - 1] = elements[i];
                }

                //decrease the size as the first element has been removed
                size--;

                //return the movie that was at the front of the queue
                return item;
            }

            //checks whether queue is empty
            public bool IsEmpty()
            {
                //returns true if there are no elements in the queue
                return size == 0;
            }

        }

        //method to find movie by specific title
        public static Movie TitleSearch(Movie[] arr, string title)
        {



            //go through each movie in the array
            for (int i = 0; i < arr.Length; i++)
            {
                //compare the current movies title with the search
                if (arr[i].Title == title)
                {
                    //if a match is found return the movie object
                    return arr[i];
                }
            }
            //if no match is found throw an exception
            throw new InvalidOperationException("no movie with this title");

        }

        public static Movie BinarySearch(Movie[] arr, string MovieID)
        {
            //sort array by movieID to ensure its in order before searching
            Array.Sort(arr, (x, y) => string.Compare(x.MovieID, y.MovieID));

            //setting left and right pointers of array
            int left = 0;
            int right = arr.Length - 1;

            //continue search while left pointer is less than or equal to the right pointer
            while (left <= right)
            {
                //calculate middle index of search range
                int mid = left + (right - left) / 2;

                //compare the middle movies id with the target movieid
                int comparison = string.Compare(arr[mid].MovieID, MovieID);

                //if the movieids match, return the movie found at the middle index
                if (comparison == 0)
                {
                    return arr[mid];
                }
                //else if target movieID is greater, narrow the search to the right half of the array
                else if (comparison < 0)
                {
                    left = mid + 1;
                }
                //if target movieID is smaller, narrow the search to the left half of the array
                else
                {
                    right = mid - 1;
                }
            }
            //if the movie with the target MovieID is not found return null
            return null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //get input from the textbox
            string searchInput = tbxTitleSearch.Text;

            // Check if input is empty
            if (string.IsNullOrWhiteSpace(searchInput))
            {
                //display warning if input is empty
                MessageBox.Show("Please enter a valid search input.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Movie foundMovie = null;

                // Check if the input is numeric (MovieID should be numeric)
                if (searchInput.StartsWith("m") && searchInput.Length > 1 && searchInput.Substring(1).All(char.IsDigit))
                {
                    // Perform binary search if the input is a MovieID (should start with 'm' and then digits)
                    foundMovie = BinarySearch(movieArray, searchInput);
                }
                else
                {
                    // if input isnt a movieID perform linear search if the input is a Title
                    foundMovie = TitleSearch(movieArray, searchInput);
                }

                // If foundMovie is not null, display it in the ListView
                if (foundMovie != null)
                {
                    var movieList = new List<Movie> { foundMovie };
                    lsvMList.ItemsSource = movieList;
                }
                else
                {
                    // If no movie is found, display a message and clear the listview
                    lsvMList.ItemsSource = null;
                    MessageBox.Show("Movie ID not found", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //private void btnSearch_Click(object sender, RoutedEventArgs e)
        //{

        //    //////// WORKS FOR TITLE SEARCH//////////////

        //    //get the search title from textbox
        //    string SearchTitle = tbxTitleSearch.Text;

        //    try
        //    {
        //        //attempt to find movie by title 
        //        Movie foundMovie = TitleSearch(movieArray, SearchTitle);

        //        //create a list to hold the found movie
        //        var movieList = new List<Movie> { foundMovie };

        //        //set listviews item source to the movie list 
        //        lsvMList.ItemsSource = movieList;

        //    }


        //    catch (InvalidOperationException ex)
        //    {
        //        //if no movie is found clear the listviews item source 
        //        lsvMList.ItemsSource = null;

        //        //show a message box showing an error
        //        MessageBox.Show(ex.Message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }


        //}

        ///////////WORKS FOR ID SEARCH//////////////
        //{
        //    string searchID = tbxTitleSearch.Text;

        //    try
        //    {
        //        // Perform Binary Search by MovieID
        //        Movie foundMovie = BinarySearch(movieArray, searchID);

        //        if (foundMovie != null)
        //        {
        //            // Display the found movie in the ListView
        //            var movieList = new List<Movie> { foundMovie };
        //            lsvMList.ItemsSource = movieList;
        //        }
        //        else
        //        {
        //            // Display a message if no movie found
        //            lsvMList.ItemsSource = null;
        //            MessageBox.Show("Movie not found", "Search Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle any unexpected errors
        //        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
        //}

        //method for a bubble sort based on movie titles
        public static void BubbleTitleSort(Movie[] arr)
        {
            int n = arr.Length;

            //outer loop runs through the array n - 1 times
            for (int i = 0; i < n - 1; i++)
            {
                //inner loop compares elements and swaps if out of order
                for (int j = 0; j < n - 1; j++)
                {
                    //compare titles alphabetically, then swaps if out of order
                    if (string.Compare(arr[j].Title, arr[j + 1].Title) > 0)
                    {
                        //swapping movies
                        Movie temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }
        //bubblesort click event
        private void btnBubbleSort_Click(object sender, RoutedEventArgs e)
        {
            //sort moive array by title using bubble sort
            BubbleTitleSort(movieArray);
            //display sorted array in the listview
            lsvMList.ItemsSource = movieArray;
        }

        //public method to sort an array of movies by release year, merge sort
        public static void MergeByYear(Movie[] arr)
        {
            //if array has 1 more no elements, its already sorted
            if (arr.Length <= 1)
                return;

            //temp array used during merging
            Movie[] temp = new Movie[arr.Length];

            //start the merge sort process
            MergeSort(arr, temp, 0, arr.Length - 1);
        }

        //method that divides the array into halves and sorts them
        private static void MergeSort(Movie[] arr, Movie[] temp, int leftStart, int rightEnd)
        {
            //if theres only one element, stop dividing
            if (leftStart >= rightEnd)
                return;

            //find the middle index to divide the array
            int middle = (leftStart + rightEnd) / 2;

            //sort the left half
            MergeSort(arr, temp, leftStart, middle);
            //sort the right half
            MergeSort(arr, temp, middle + 1, rightEnd);
            //merge both halves together
            MergeHalves(arr, temp, leftStart, rightEnd);
        }

        //method to merge both sorted halves of the array
        private static void MergeHalves(Movie[] arr, Movie[] temp, int leftStart, int rightEnd)
        {

            int leftEnd = (leftStart + rightEnd) / 2;  //end of left half
            int rightStart = leftEnd + 1;  //start of right half

            int left = leftStart;  //pointer for left
            int right = rightStart;  //pointer for right
            int index = leftStart;  //pointer for merged result 

            //while there are still elements in both halves to compare
            while (left <= leftEnd && right <= rightEnd)
            {
                //compare the release years of movies in left and right halves
                if (arr[left].ReleaseYear <= arr[right].ReleaseYear)
                {
                    //if the left movie was released earlier or in the same year, place it in the temp array
                    temp[index] = arr[left];
                    //move to the next movie in the left half
                    left++;
                }
                else
                {
                    //else the right movie was released earlier, so place it in the temp array
                    temp[index] = arr[right];
                    //move to the next position in the temp array
                    right++;
                }
                //move to the next position in the temp array
                index++;
            }

            //copy any remaining elements from both halfs
            Array.Copy(arr, left, temp, index, leftEnd - left + 1);
            Array.Copy(arr, right, temp, index, rightEnd - right + 1);

            //copy merged result back into the array
            Array.Copy(temp, leftStart, arr, leftStart, rightEnd - leftStart + 1);

        }

        //merge sort button click event
        private void btnMergeSort_Click(object sender, RoutedEventArgs e)
        {
            //perform merge sort by release year
            MergeByYear(movieArray);

            //refresh listview with the sorted movie array
            lsvMList.ItemsSource = null;
            lsvMList.ItemsSource = movieArray;
        }

        //method to borrow a movie
        private void BorrowMovie(Movie selectedMovie, string userName)
        {
            //check if movie is available
            if (selectedMovie.Availability)
            {
                //mark the movie as not available
                selectedMovie.Availability = false;

                //assign the current borrower to the user
                selectedMovie.CurrentBorrower = userName;

                //show a message indicating that the user has borrowed the movie
                MessageBox.Show($"'{selectedMovie.Title}' is currently unavailable. {userName} has been added to the waitlist.", "waitlisted");
            }
        }

        //method to handle movie return
        private void MovieReturn(Movie selectedMovie)
        {
            //check if there is anyone in the waitlist for the movie
            if (selectedMovie.Waitlist.Count == 0)
            {
                //if no one is, mark movie as available
                selectedMovie.Availability = true;

                //clear the current borrower info
                selectedMovie.CurrentBorrower = null;

                //notify that the movie has been returned and it available
                MessageBox.Show($"' {selectedMovie.Title}' has been returned and is now available, ", "returned");
            }
            else
            {
                //if there is a waitlist, deqeue the next user
                string nextUser = selectedMovie.Waitlist.Dequeue();

                //assign the movie to the next user
                selectedMovie.CurrentBorrower = nextUser;

                //keep the movie marked as unavailable since its now borrowed
                selectedMovie.Availability = false;

                //message box to show the movie has been assigned to the next user
                MessageBox.Show($"'{selectedMovie.Title}' has been automatically assigned to {nextUser}.", "assigned to next user");
            }

        }

        //event handler for the borrow movie button
        private void btnBorrowMovie_Click(object sender, RoutedEventArgs e)
        {
            //check if movie is selected from listview
            if (lsvMList.SelectedItem is Movie selectedMovie)
            {
                //check if it is borrowed
                if (!selectedMovie.IsBorrowed)
                {
                    //mark movie as borrowed 
                    selectedMovie.IsBorrowed = true;

                    //notify the user borrow was successfull
                    MessageBox.Show($"{selectedMovie.Title} has been borrowed successfully.");
                }
                else
                {
                    //get the entered username from textbox
                    string user = tbxUsername.Text.Trim();

                    //if username is provided, add user to the waitlist
                    if (!string.IsNullOrEmpty(user))
                    {
                        //notify that the user has been added to the waitlist
                        MessageBox.Show($"{selectedMovie.Title} is currently unavailable. {user} has been added to the waiting list.");
                    }
                    else
                    {
                        //make user enter a username before entering
                        MessageBox.Show("please enter a username before borrowing");
                    }
                }
                //refresh list view
                RefreshListView();
            }
            else
            {
                //make user select a movie 
                MessageBox.Show("please select a movie to borrow");
            }
        }

        //event for return movie button click
        private void btnReturnMovie_Click(object sender, RoutedEventArgs e)
        {
            //check if a movie is selected from listview
            if (lsvMList.SelectedItem is Movie selectedMovie)
            {
                //check if movie is borrowed
                if (selectedMovie.IsBorrowed)
                {
                    //check if there are users in the waitlist
                    if (selectedMovie.Waitlist.Count > 0)
                    {
                        //dequeue  the next user from waitlist
                        string nextUser = selectedMovie.Waitlist.Dequeue();

                        //notify that the movie has been reassigned to the next user in the queue
                        MessageBox.Show($"{selectedMovie.Title} has been returned and is now borrowed by {nextUser}");
                    }
                    else
                    {
                        //if theres no waitlist mark movie as available
                        selectedMovie.IsBorrowed = false;

                        //notify that the movie has been returned and available
                        MessageBox.Show($"{selectedMovie.Title} has been returned and is now available.");
                    }

                    //refresh list view
                    RefreshListView();
                }
                else
                {
                    //let user know selected movie is not borrowed
                    MessageBox.Show("this movie is not currently borrowed.");
                }
            }
            else
            {
                //message to lt user know they must select a movie to return
                MessageBox.Show("please select a movie to return");
            }

        }

        //method to refresh the listbiew 
        private void RefreshListView()
        {
            //clear the current binding to force the listview to reset
            lsvMList.ItemsSource = null;

            //reassign the data source so the listview reloads the updated movie array
            lsvMList.ItemsSource = movieArray;
        }

        //method to allow import movies from a JSON or XML file
        private void ImportMovies()
        {
            //create and configure a dialog to allow the user to select a JSON or XML file
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";

            //show dialog and proceed if user selects a file
            if (openFileDialog.ShowDialog() == true)
            {
                //get the selected files path
                string filePath = openFileDialog.FileName;

                //if the selected file is a JSON file
                if (filePath.EndsWith(".json"))
                {
                    //read the contents of the file
                    string jsonString = File.ReadAllText(filePath);

                    //deserialize the JSON string into a movie array
                    movieArray = JsonSerializer.Deserialize<Movie[]>(jsonString);

                    //notify the user of successful import
                    MessageBox.Show("Movies imported from JSON successfully.");
                }
                //else if the selected file is an XML file
                else if (filePath.EndsWith(".xml"))
                {
                    //create an XML serializer for the movie array
                    XmlSerializer serializer = new XmlSerializer(typeof(Movie[]));

                    //open the file stream and deserialize the contents into the movieArray
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        movieArray = (Movie[])serializer.Deserialize(fs);
                    }
                    //message to showe successful import
                    MessageBox.Show("Movies imported from XML successfully.");
                }
                else
                {
                    //if file isnt XML or JSON give error message
                    MessageBox.Show("Unsupported file type.");
                }
                //refresh list view
                RefreshListView();
            }
        }

        //method to export movie list to JSON or XML file
        private void ExportMovies()
        {
            //create a save file dialog to let the user choose file format
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|XML files (*.xml)|*.xml";

            //show save dialog and proceed if user clicks save
            if (saveFileDialog.ShowDialog() == true)
            {
                //get file path where user is saving
                string filePath = saveFileDialog.FileName;

                //if file chosen is JSON
                if (filePath.EndsWith(".json"))
                {
                    //convert tje movie array to a JSON string
                    string jsonString = JsonSerializer.Serialize(movieArray);

                    //write the JSON string to the specified file
                    File.WriteAllText(filePath, jsonString);

                    //messagebox to show export was successful
                    MessageBox.Show("Movies exported to JSON successfully.");
                }
                //if file chosen is XML
                else if (filePath.EndsWith(".xml"))
                {
                    //create an XML serializer fpr the movie array 
                    XmlSerializer serializer = new XmlSerializer(typeof(Movie[]));

                    //open a filestream and serialize the movie array into the file
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        serializer.Serialize(fs, movieArray);
                    }
                    //message to show export was successful
                    MessageBox.Show("Movies exported to XML successfully.");
                }
                else
                {
                    //message to show file type isnt supported
                    MessageBox.Show("Unsupported file type.");
                }
            }
        }

        //import movie click event
        private void btnImportMovies_Click(object sender, RoutedEventArgs e)
        {
            //calls method
            ImportMovies();
        }

        //export movie click event
        private void btnExportMovies_Click(object sender, RoutedEventArgs e)
        {
            //calls method
            ExportMovies();
        }

        //button to clear listview
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lsvMList.ItemsSource = null;
        }

        private void btnAddMovie_Click(object sender, RoutedEventArgs e)
        {
            string movieid = tbxMovieID.Text.Trim();
            string title = tbxTitle.Text.Trim();
            string director = tbxDirector.Text.Trim();
            string genre = tbxGenre.Text.Trim();
            bool yearParsed = int.TryParse(tbxYear.Text.Trim(), out int year);

            // Validate all fields
            if (string.IsNullOrWhiteSpace(movieid) ||
                string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(director) ||
                string.IsNullOrWhiteSpace(genre) ||
                !yearParsed)
            {
                MessageBox.Show("Please fill in valid movie information.");
                return;
            }

            // Create and add movie
            Movie newMovie = new Movie
            {
                MovieID = movieid,
                Title = title,
                Director = director,
                Genre = genre,
                ReleaseYear = year,
                Availability = true,
                Waitlist = new Queue<string>()
            };

            movieArray = movieArray.Append(newMovie).ToArray();
            RefreshListView();

            MessageBox.Show("Movie added successfully.");
            ClearMovieInputFields();
        }

        private void ClearMovieInputFields()
        {
            tbxMovieID.Clear();
            tbxTitle.Clear();
            tbxDirector.Clear();
            tbxYear.Clear();
            tbxGenre.Clear();
        }
    }
}
