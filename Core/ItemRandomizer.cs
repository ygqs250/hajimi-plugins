using System;
using System.Collections.Generic;
using System.Linq;

public static class ItemRandomizer
{
    // 预缓存有效物品列表和权重数据
    private static readonly List<ItemType> _validItems;
    private static readonly List<float> _cumulativeWeights;
    private static readonly float _totalWeight;
    private static readonly Random _random = new Random();

    // 配置特定物品的概率（未配置的物品默认权重为1）
    private static readonly Dictionary<ItemType, float> _customWeights = new Dictionary<ItemType, float>
    {
        { ItemType.GunSCP127, 0f },    // 概率为0
        { ItemType.MicroHID, 0.5f }, // 50%概率
    };

    // 静态构造函数：初始化时预计算所有数据
    static ItemRandomizer()
    {
        // 获取所有有效物品（排除None）
        _validItems = Enum.GetValues(typeof(ItemType))
            .Cast<ItemType>()
            .Where(item => item != ItemType.None)
            .ToList();

        // 预计算累积权重列表
        _cumulativeWeights = new List<float>();
        float cumulative = 0f;
        foreach (var item in _validItems)
        {
            float weight = _customWeights.TryGetValue(item, out float w) ? w : 1f;
            cumulative += weight;
            _cumulativeWeights.Add(cumulative);
        }
        _totalWeight = cumulative;
    }

    public static ItemType GetRandomItem()
    {
        if (_totalWeight <= 0)
            return ItemType.None; // 所有物品概率为0时返回None

        float randomValue = (float)_random.NextDouble() * _totalWeight;

        // 二分查找优化（将时间复杂度从O(n)降到O(log n)）
        int index = _cumulativeWeights.BinarySearch(randomValue);
        if (index < 0)
            index = ~index; // 取补码得到插入位置

        return index < _validItems.Count ? _validItems[index] : ItemType.None;
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;

//public static class ItemRandomizer
//{
//    // 定义物品概率字典（默认概率为1，需要调整的物品在此设置）
//    private static readonly Dictionary<ItemType, float> _itemWeights = new Dictionary<ItemType, float>
//    {
//        { ItemType.SCP207, 0f },    // SCP207概率为0
//        { ItemType.MicroHID, 0.5f }, // 示例：MicroHID概率为50%
//        { ItemType.GunCOM15, 2f }   // 示例：COM15手枪概率为200%
//    };

//    public static ItemType GetRandomItem()
//    {
//        // 获取所有有效物品（排除None）
//        var validItems = Enum.GetValues(typeof(ItemType))
//            .Cast<ItemType>()
//            .Where(item => item != ItemType.None)
//            .ToList();

//        // 计算总权重
//        float totalWeight = validItems.Sum(item =>
//            _itemWeights.TryGetValue(item, out float weight) ? weight : 1f);

//        // 生成随机值
//        float randomValue = (float)new Random().NextDouble() * totalWeight;

//        // 根据权重选择物品
//        float currentWeight = 0f;
//        foreach (var item in validItems)
//        {
//            float weight = _itemWeights.TryGetValue(item, out float w) ? w : 1f;
//            currentWeight += weight;

//            if (randomValue <= currentWeight)
//                return item;
//        }

//        return ItemType.None; // 理论上不会执行到这里
//    }
//}