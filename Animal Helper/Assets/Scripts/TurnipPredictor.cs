using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PossiblePrice
{
    public int min;
    public int max;
    public PossiblePrice(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
}

public class Possibility
{
    public int pattern = -1;
    public string description;
    public List<PossiblePrice> prices = new List<PossiblePrice>();
    public int highest;
    public int lowest;
}




public class TurnipPredictor : MonoBehaviour
{


    public List<Possibility> AnalyzePossibilities(int[] sellPrices)
    {
        List<Possibility> posibilities = new List<Possibility>();

        List<Possibility> list0 = GeneratePattern0(sellPrices);
        List<Possibility> list1 = GeneratePattern1(sellPrices);
        List<Possibility> list2 = GeneratePattern2(sellPrices);
        List<Possibility> list3 = GeneratePattern3(sellPrices);

        posibilities.AddRange(list0);
        posibilities.AddRange(list1);
        posibilities.AddRange(list2);
        posibilities.AddRange(list3);


        string msg = "";
        foreach (Possibility possibility in posibilities)
        {

            int[] weekMin = new int[12];
            int[] weekMax = new int[12];
            for (int i = 0; i < 12; i++)
            {
                weekMin[i] = possibility.prices[i + 2].min;
                weekMax[i] = possibility.prices[i + 2].max;
            }
            possibility.lowest = Mathf.Min(weekMin);
            possibility.highest = Mathf.Max(weekMax);


            string text = possibility.description + ": ";

            foreach (PossiblePrice possiblePrice in possibility.prices)
            {
                text += (possiblePrice.min + "~" + possiblePrice.max + "  ");
            }

            text += " H: " + possibility.highest;
            text += " L: " + possibility.lowest;

            msg += text + "\n";


        }

        Debug.Log(msg);


        return posibilities;
    }

    void Start()
    {
        // int[] testPrice = new int[14];
        // testPrice[0] = 100;


        // AnalyzePossibilities(testPrice);



    }




    float MinRateFromGivenAndBase(int given_price, int buy_price)
    {
        return 10000f * (given_price - 1) / buy_price;
    }
    float MaxRateFromGivenAndBase(int given_price, int buy_price)
    {
        return 10000f * given_price / buy_price;
    }


    List<Possibility> GeneratePattern0(int[] sellPrices)
    {
        List<Possibility> possibilities = new List<Possibility>();

        for (var dec_phase_1_len = 2; dec_phase_1_len < 4; dec_phase_1_len++)
        {
            for (var high_phase_1_len = 0; high_phase_1_len < 7; high_phase_1_len++)
            {
                for (var high_phase_3_len = 0; high_phase_3_len < (7 - high_phase_1_len - 1 + 1); high_phase_3_len++)
                {
                    Possibility possibility = GeneratePattern0WithLengths(sellPrices, high_phase_1_len, dec_phase_1_len, 7 - high_phase_1_len - high_phase_3_len, 5 - dec_phase_1_len, high_phase_3_len);
                    if (possibility != null)
                        possibilities.Add(possibility);
                }
            }
        }
        return possibilities;
    }
    List<Possibility> GeneratePattern1(int[] sellPrices)
    {
        List<Possibility> possibilities = new List<Possibility>();
        for (var peak_start = 3; peak_start < 10; peak_start++)
        {
            Possibility possibility = GeneratePattern1WithPeak(sellPrices, peak_start);
            if (possibility != null)
                possibilities.Add(possibility);
        }
        return possibilities;
    }

    List<Possibility> GeneratePattern2(int[] sellPrices)
    {
        List<Possibility> possibilities = new List<Possibility>();

        Possibility possibility = GeneratePattern2Only(sellPrices);
        if (possibility != null)
            possibilities.Add(possibility);

        return possibilities;
    }

    List<Possibility> GeneratePattern3(int[] sellPrices)
    {
        List<Possibility> possibilities = new List<Possibility>();

        for (var peak_start = 2; peak_start < 10; peak_start++)
        {
            Possibility possibility = GeneratePattern3WithPeak(sellPrices, peak_start);
            if (possibility != null)
                possibilities.Add(possibility);
        }
        return possibilities;
    }


    Possibility GeneratePattern0WithLengths(int[] given_prices,
                                            int high_phase_1_len, int dec_phase_1_len, int high_phase_2_len, int dec_phase_2_len, int high_phase_3_len)
    {
        Possibility possibility = new Possibility();
        possibility.pattern = 0;
        possibility.description = "波形";
        int buy_price = given_prices[0];
        PossiblePrice buyPrice = new PossiblePrice(buy_price, buy_price);
        possibility.prices.Add(buyPrice);
        possibility.prices.Add(buyPrice);



        // High Phase 1
        for (var i = 2; i < 2 + high_phase_1_len; i++)
        {
            var min_pred = Mathf.FloorToInt(0.9f * buy_price);
            var max_pred = Mathf.CeilToInt(1.4f * buy_price);
            if ((given_prices[i]) > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
            }

            PossiblePrice price = new PossiblePrice(min_pred, max_pred);
            possibility.prices.Add(price);
        }


        // Dec Phase 1
        var min_rate = 6000f;
        var max_rate = 8000f;
        for (var i = 2 + high_phase_1_len; i < 2 + high_phase_1_len + dec_phase_1_len; i++)
        {
            var min_pred = Mathf.Floor(min_rate * buy_price / 10000f);
            var max_pred = Mathf.Ceil(max_rate * buy_price / 10000f);


            if ((given_prices[i]) > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
                min_rate = MinRateFromGivenAndBase(given_prices[i], buy_price);
                max_rate = MaxRateFromGivenAndBase(given_prices[i], buy_price);
            }

            PossiblePrice price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);

            min_rate -= 1000;
            max_rate -= 400;
        }


        // High Phase 2
        for (var i = 2 + high_phase_1_len + dec_phase_1_len; i < 2 + high_phase_1_len + dec_phase_1_len + high_phase_2_len; i++)
        {
            var min_pred = Mathf.Floor(0.9f * buy_price);
            var max_pred = Mathf.Ceil(1.4f * buy_price);
            if ((given_prices[i]) > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
            }

            PossiblePrice price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);
        }


        // Dec Phase 2
        min_rate = 6000f;
        max_rate = 8000f;
        for (var i = 2 + high_phase_1_len + dec_phase_1_len + high_phase_2_len; i < 2 + high_phase_1_len + dec_phase_1_len + high_phase_2_len + dec_phase_2_len; i++)
        {
            var min_pred = Mathf.Floor(min_rate * buy_price / 10000f);
            var max_pred = Mathf.Ceil(max_rate * buy_price / 10000f);

            if ((given_prices[i]) > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
                min_rate = MinRateFromGivenAndBase(given_prices[i], buy_price);
                max_rate = MaxRateFromGivenAndBase(given_prices[i], buy_price);
            }

            PossiblePrice price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);

            min_rate -= 1000;
            max_rate -= 400;
        }

        // High Phase 3
        if (2 + high_phase_1_len + dec_phase_1_len + high_phase_2_len + dec_phase_2_len + high_phase_3_len != 14)
        {
            Debug.LogError("Phase lengths don't add up");
        }
        for (var i = 2 + high_phase_1_len + dec_phase_1_len + high_phase_2_len + dec_phase_2_len; i < 14; i++)
        {
            var min_pred = Mathf.Floor(0.9f * buy_price);
            var max_pred = Mathf.Ceil(1.4f * buy_price);
            if ((given_prices[i]) > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
            }

            PossiblePrice price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);
        }

        return possibility;
    }



    Possibility GeneratePattern1WithPeak(int[] given_prices, int peak_start)
    {
        Possibility possibility = new Possibility();
        possibility.pattern = 1;
        possibility.description = "三期";
        int buy_price = given_prices[0];
        PossiblePrice buyPrice = new PossiblePrice(buy_price, buy_price);
        possibility.prices.Add(buyPrice);
        possibility.prices.Add(buyPrice);


        var min_rate = 8500f;
        var max_rate = 9000f;

        for (var i = 2; i < peak_start; i++)
        {
            var min_pred = Mathf.Floor(min_rate * buy_price / 10000f);
            var max_pred = Mathf.Ceil(max_rate * buy_price / 10000f);


            if (given_prices[i] > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
                min_rate = MinRateFromGivenAndBase(given_prices[i], buy_price);
                max_rate = MaxRateFromGivenAndBase(given_prices[i], buy_price);
            }

            PossiblePrice price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);

            min_rate -= 500;
            max_rate -= 300;
        }


        // Now each day is independent of next
        var min_randoms = new float[] { 0.9f, 1.4f, 2.0f, 1.4f, 0.9f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f, 0.4f };
        var max_randoms = new float[] { 1.4f, 2.0f, 6.0f, 2.0f, 1.4f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f, 0.9f };
        for (var i = peak_start; i < 14; i++)
        {
            var min_pred = Mathf.Floor(min_randoms[i - peak_start] * buy_price);
            var max_pred = Mathf.Ceil(max_randoms[i - peak_start] * buy_price);

            if (given_prices[i] > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
            }

            PossiblePrice price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);
        }

        return possibility;
    }


    Possibility GeneratePattern2Only(int[] given_prices)
    {
        Possibility possibility = new Possibility();
        possibility.pattern = 2;
        possibility.description = "遞減";
        int buy_price = given_prices[0];
        PossiblePrice buyPrice = new PossiblePrice(buy_price, buy_price);
        possibility.prices.Add(buyPrice);
        possibility.prices.Add(buyPrice);


        var min_rate = 8500f;
        var max_rate = 9000f;
        for (var i = 2; i < 14; i++)
        {
            var min_pred = Mathf.Floor(min_rate * buy_price / 10000f);
            var max_pred = Mathf.Ceil(max_rate * buy_price / 10000f);


            if (given_prices[i] > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
                min_rate = MinRateFromGivenAndBase(given_prices[i], buy_price);
                max_rate = MaxRateFromGivenAndBase(given_prices[i], buy_price);
            }

            PossiblePrice price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);

            min_rate -= 500;
            max_rate -= 300;
        }

        return possibility;
    }


    Possibility GeneratePattern3WithPeak(int[] given_prices, int peak_start)
    {
        Possibility possibility = new Possibility();
        possibility.pattern = 3;
        possibility.description = "四期";
        int buy_price = given_prices[0];
        PossiblePrice buyPrice = new PossiblePrice(buy_price, buy_price);
        possibility.prices.Add(buyPrice);
        possibility.prices.Add(buyPrice);



        var min_rate = 4000f;
        var max_rate = 9000f;
        float min_pred, max_pred;
        PossiblePrice price;

        for (var i = 2; i < peak_start; i++)
        {
            min_pred = Mathf.Floor(min_rate * buy_price / 10000f);
            max_pred = Mathf.Ceil(max_rate * buy_price / 10000f);


            if (given_prices[i] > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
                min_rate = MinRateFromGivenAndBase(given_prices[i], buy_price);
                max_rate = MaxRateFromGivenAndBase(given_prices[i], buy_price);
            }

            price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);

            min_rate -= 500;
            max_rate -= 300;
        }

        // The peak
        for (var i = peak_start; i < peak_start + 2; i++)
        {
            min_pred = Mathf.Floor(0.9f * buy_price);
            max_pred = Mathf.Ceil(1.4f * buy_price);
            if (given_prices[i] > 0)
            {
                if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                {
                    // Given price is out of predicted range, so this is the wrong pattern
                    return null;
                }
                min_pred = given_prices[i];
                max_pred = given_prices[i];
            }

            price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
            possibility.prices.Add(price);
        }

        // Main spike 1
        min_pred = Mathf.Floor(1.4f * buy_price) - 1;
        max_pred = Mathf.Ceil(2.0f * buy_price) - 1;
        if ((given_prices[peak_start + 2]) > 0)
        {
            if (given_prices[peak_start + 2] < min_pred || given_prices[peak_start + 2] > max_pred)
            {
                // Given price is out of predicted range, so this is the wrong pattern
                return null;
            }
            min_pred = given_prices[peak_start + 2];
            max_pred = given_prices[peak_start + 2];
        }
        price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
        possibility.prices.Add(price);


        // Main spike 2
        min_pred = possibility.prices[peak_start + 2].min;
        max_pred = Mathf.Ceil(2.0f * buy_price);
        if ((given_prices[peak_start + 3]) > 0)
        {
            if (given_prices[peak_start + 3] < min_pred || given_prices[peak_start + 3] > max_pred)
            {
                // Given price is out of predicted range, so this is the wrong pattern
                return null;
            }
            min_pred = given_prices[peak_start + 3];
            max_pred = given_prices[peak_start + 3];
        }
        price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
        possibility.prices.Add(price);




        // Main spike 3
        min_pred = Mathf.Floor(1.4f * buy_price) - 1;
        max_pred = possibility.prices[peak_start + 3].max - 1;
        if (given_prices[peak_start + 4] > 0)
        {
            if (given_prices[peak_start + 4] < min_pred || given_prices[peak_start + 4] > max_pred)
            {
                // Given price is out of predicted range, so this is the wrong pattern
                return null;
            }
            min_pred = given_prices[peak_start + 4];
            max_pred = given_prices[peak_start + 4];
        }
        price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
        possibility.prices.Add(price);



        if (peak_start + 5 < 14)
        {
            min_rate = 4000f;
            max_rate = 9000f;

            for (var i = peak_start + 5; i < 14; i++)
            {
                min_pred = Mathf.Floor(min_rate * buy_price / 10000f);
                max_pred = Mathf.Ceil(max_rate * buy_price / 10000f);


                if (given_prices[i] > 0)
                {
                    if (given_prices[i] < min_pred || given_prices[i] > max_pred)
                    {
                        // Given price is out of predicted range, so this is the wrong pattern
                        return null;
                    }
                    min_pred = given_prices[i];
                    max_pred = given_prices[i];
                    min_rate = MinRateFromGivenAndBase(given_prices[i], buy_price);
                    max_rate = MaxRateFromGivenAndBase(given_prices[i], buy_price);
                }

                price = new PossiblePrice(Mathf.RoundToInt(min_pred), Mathf.RoundToInt(max_pred));
                possibility.prices.Add(price);

                min_rate -= 500;
                max_rate -= 300;
            }
        }


        return possibility;

    }





}
