# Construction Line code challenge

The code challenge consists in the implementation of a simple search engine for shirts.

## What to do?
Shirts are in different sizes and colors. As described in the Size.cs class, there are three sizes: small, medium and large, and five different colors listed in Color.cs class.

The search specifies a range of sizes and colors in SearchOptions.cs. For example, for small, medium and red the search engine should return shirts that are either small or medium in size and are red in color. In this case, the SearchOptions should look like:

```
{
    Sizes = List<Size> {Size.Small, Size.Medium},
    Colors = List<Color> {Color.Red}
}
```

The results should include, as well as the shirts matching the search options, the total count for each search option taking into account the options that have been selected. For example, if there are two shirts, one small and red and another medium and blue, if the search options are small size and red color, the results (captured in SearchResults.cs) with total count for each option should be:
```
{
    Shirts = List<Shirt> { SmallRedShirt },
    SizeCounts = List<SizeCount> { Small(1), Medium(0), Large(0)},
    ColorCounts = List<ColorCount> { Red(1), Blue(0), Yellow(0), White(0), Black(0)}
}
```

The search engine logic sits in SearchEngine.cs and should be implemented by the candidate. Feel free to use any additional data structures, classes or libraries to prepare the data before the actual search. The initalisation of these should sit in the constructor of the search engine.

There are two tests in the test project; one simple search for red shirts out of a total of three, and another one which tests the performance of the search algorithm through 50.000 random shirts of all sizes and colors which measures how long it takes to perform the search algorithm. A reasonable implementation should not take more than 100 ms to return the results.

## Procedure
We would like you to send us a link to a git repository that we can access with your implementation.

The whole exercise should not take more than an hour to implement.

## Solution Notes
The strategy employed in this solution is a simple linear search and the default implementation uses linq. Two other implementations were written in an effort to boost performance, one does the search in parallel, the other avoids using linq. However, as can be seen by the results below the parallel implementation significantly reduced performance and the non linq implementation showed little or no improvement over the default.

N.B. The two methods AssertSizeCounts/AssertColorCounts in SearchEngineTestsBase have been updatd to properly reflect the search criteria. 

## Performance
Performance was tested with the release build configuration using an average of 5 runs and a 4 core machine.

#Linq Implementation (Search(...))

5000 Shirts: 94ms
10,000,000 14.5s

#Linq Parallel Implementation (SearchParallel(...))

5000 Shirts: 144ms
10,000,000 16.4s

#Non Linq Implementation (SearchWithoutLinq(...))
5000 Shirts: 93ms
10,000,000 14.8s