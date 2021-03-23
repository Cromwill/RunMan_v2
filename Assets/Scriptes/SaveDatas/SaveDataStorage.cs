using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaveDataStorage
{
    public static void SaveBuyableObject(IBuyableObject buyable, bool isInInventory = true)
    {
        if (buyable.Type == "avatar")
        {
            SaveAvatar(buyable);
        }
        else
        {
            SaveItem(buyable as IItem, isInInventory);
        }
        PlayerPrefs.Save();
    }

    public static void SaveCurrentRunner(IBuyableObject avatar)
    {
        int[] savedAvatars = LoadOpenedRunnersIds();

        if(savedAvatars.Contains(avatar.Id))
        {
            PlayerPrefs.SetInt("CurrentAvatarId", avatar.Id);
        }

        PlayerPrefs.Save();
    }

    public static int[] LoadOpenedRunnersIds()
    {
        int[] avatarIds = new int[PlayerPrefs.GetInt("OpenAvatarCount")];

        for (int i = 0; i < avatarIds.Length; i++)
        {
            if (PlayerPrefs.HasKey("Avatar_number_" + (i + 1)))
            {
                avatarIds[i] = (PlayerPrefs.GetInt("Avatar_number_" + (i + 1)));
            }
        }

        return avatarIds;
    }

    public static int LoadCurrentRunnersId() => PlayerPrefs.GetInt("CurrentAvatarId");

    public static void SaveSkills(string skillName, int value)
    {
        PlayerPrefs.SetInt("SavedSkill_" + skillName, value);
        PlayerPrefs.Save();
    }

    public static int LoadSkills(string skillName)
    {
        return PlayerPrefs.GetInt("SavedSkill_" + skillName);
    }

    public static void SaveScore(Score score)
    {
        PlayerPrefs.SetInt("Money", score.Money);
        PlayerPrefs.SetInt("Coins", score.Coins);
        PlayerPrefs.Save();
    }

    public static void AddScore(Score score)
    {
        Score oldScore = LoadScore();
        Score newScore = new Score(oldScore.Money + score.Money, oldScore.Coins + score.Coins);
        SaveScore(newScore);
    }

    public static Score LoadScore()
    {
        return new Score(PlayerPrefs.GetInt("Money"), PlayerPrefs.GetInt("Coins"));
    }

    public static void SaveItem(IItem item, bool isItemHave)
    {
        PlayerPrefs.SetString("Haveable_item_" + item.GetItemName, isItemHave.ToString());
        PlayerPrefs.Save();
    }

    public static bool ItemContain(IItem item)
    {
        if (PlayerPrefs.HasKey("Haveable_item_" + item.GetItemName))
        {
            return PlayerPrefs.GetString("Haveable_item_" + item.GetItemName) == true.ToString();
        }
        else
            return false;
    }

    public static bool HasKeyBuyableObjecty(IBuyableObject buyabl)
    {
        if (buyabl.Type == "avatar")
            return PlayerPrefs.HasKey("CurrentAvatarId");
        else
            return false;

    }

    private static void SaveAvatar(IBuyableObject buyable)
    {
        int avatarsCount = PlayerPrefs.GetInt("OpenAvatarCount");
        int[] savedAvatars = LoadOpenedRunnersIds();
        bool isDubleAvatar = false;

        if (avatarsCount != 0)
        {
            foreach (int id in savedAvatars)
            {
                if (id == buyable.Id)
                    isDubleAvatar = true;
            }

            if (!isDubleAvatar)
            {
                WriteAvatarsData(avatarsCount, buyable.Id);
            }
        }
        else
        {
            WriteAvatarsData(avatarsCount, buyable.Id);
        }
    }

    private static void WriteAvatarsData(int count, int id)
    {
        int avatarCoutn = count + 1;
        PlayerPrefs.SetInt("Avatar_number_" + avatarCoutn, id);
        PlayerPrefs.SetInt("CurrentAvatarId", id);
        PlayerPrefs.SetInt("OpenAvatarCount", avatarCoutn);
    }

}

public class Score : IEquatable<Score>, IComparable<Score>
{
    public int Money { get; private set; }
    public int Coins { get; private set; }

    public Score(int money, int coins)
    {
        Money = money;
        Coins = coins;
    }

    public override int GetHashCode() => (Money, Coins).GetHashCode();

    public bool Equals(Score other)
    {
        if (other == null)
            return false;
        if (Money == other.Money && Coins == other.Coins)
            return true;
        else
            return false;
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        Score score = obj as Score;
        if (score == null)
            return false;
        else
            return Equals(score);
    }

    public int CompareTo(Score other)
    {
        if (other == null) return 1;
        int moneyResult = Money.CompareTo(other.Money);
        int coinsResult = Coins.CompareTo(other.Coins);

        return Mathf.Min(moneyResult, coinsResult);
    }

    public static bool operator ==(Score score1, Score score2)
    {
        if (((object)score1) == null || ((object)score2) == null)
            return System.Object.Equals(score1, score2);
        return score1.Equals(score2);
    }

    public static bool operator !=(Score score1, Score score2)
    {
        if (((object)score1) == null || ((object)score2) == null)
            return !System.Object.Equals(score1, score2);

        return !(score1.Equals(score2));
    }

    public static bool operator >(Score score1, Score score2)
    {
        return score1.Money.CompareTo(score2.Money) == 1 && score1.Coins.CompareTo(score2.Coins) == 1;
    }

    public static bool operator <(Score score1, Score score2)
    {
        return score1.Money.CompareTo(score2.Money) == -1 && score1.Coins.CompareTo(score2.Coins) == -1;
    }
    public static bool operator >=(Score score1, Score score2)
    {
        return score1.Money.CompareTo(score2.Money) >= 0 && score1.Coins.CompareTo(score2.Coins) >= 0;
    }
    public static bool operator <=(Score score1, Score score2)
    {
        return score1.Money.CompareTo(score2.Money) <= 0 && score1.Coins.CompareTo(score2.Coins) <= 0;
    }

    public static Score operator +(Score score) => score;
    public static Score operator -(Score score) => new Score(-score.Money, -score.Coins);
    public static Score operator +(Score score1, Score score2) => new Score(score1.Money + score2.Money, score1.Coins + score2.Coins);
    public static Score operator -(Score score1, Score score2) => new Score(score1.Money - score2.Money, score1.Coins - score2.Coins);
}

public enum SkillType
{
    RunningSpeed,
    SwingSpeed,
    RateOfFire,
    Damage
}

