using UnityEngine;

public class InventoryList<T> where T:class
{
    private T _items;
    public T item
    {
        get { return _items; }
    }
    public InventoryList()
    {
        Debug.Log("Generic list initaliezd...");
    }
    public void SetItem(T newItem)
    {
        _items = newItem;
        Debug.Log("Item added to inventory: " + _items);
    }
}
