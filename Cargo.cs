using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using VRageMath;
using VRage.Game;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Ingame;
using Sandbox.Game.EntityComponents;
using VRage.Game.Components;
using VRage.Collections;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
public sealed class Program : MyGridProgram
{
    
    const string TAG_LCD = "[INVENTORY_LCD]";

    List<IMyTextPanel> LCDs = new List<IMyTextPanel>();
    List<IMyCargoContainer> CargoContainers = new List<IMyCargoContainer>();


    public Program()
    {
        Runtime.UpdateFrequency = UpdateFrequency.Update1;
    }

    public void Main(string args)
    {

        GridTerminalSystem.GetBlocksOfType(LCDs);
        GridTerminalSystem.GetBlocksOfType(CargoContainers);

        PrintAllInventorysData(LCDs, CargoContainers, TAG_LCD);

    }

    /// <summary>
    /// Ищет по всем LCD по тегу в имени. И выводит на них содержимое переданого списка контейнеров.
    /// </summary>
    /// <param name="Displays">Список LCD</param>
    /// <param name="Containers">Список контейнеров</param>
    /// <param name="teg">Тег в [] скобках (тег добавлять в конец имени)</param>
    public void PrintAllInventorysData(List<IMyTextPanel> Displays,  List<IMyCargoContainer> Containers, string teg) {

        int lcdInd = 0;
        foreach (IMyTextPanel Display in Displays)
        {
            lcdInd++;

            Array cusName = Display.CustomName.Split('[');
            string LCD_TAG;

            try
            { // Отлавливаем Exception если не получится сосплитить теговый символ
                LCD_TAG = "[" + cusName.GetValue(1).ToString();
            }
            catch (Exception)
            {
                if (lcdInd == Displays.Count)
                {
                    // Если ни у одного LCD нет тега то вышибаем ошибку и останавливаем скрипт
                    Echo(ErrorStr(1));
                    Runtime.UpdateFrequency = UpdateFrequency.None;
                }
                continue;
            }

            if (LCD_TAG == teg)
            {
                Display.WriteText("Содержимое инвентарей", false);
                foreach (IMyCargoContainer Container in Containers)
                {
                    foreach (string InvData in GetInventoryData(Container))
                    {
                        Display.WriteText("\n" + InvData, true);
                    }
                }
            }
        }

    }

    /// <summary>
    /// Метод возвращает содержимое контейнера (Тип содержимого: 10)
    /// </summary>
    /// <param name="Container">Контейр типа IMyCargoContainer</param>
    public List<string> GetInventoryData(IMyCargoContainer Container) {
        List<string> result = new List<string>();
        List<MyInventoryItem> InventotyItems = new List<MyInventoryItem>();
        
        InventotyItems.Clear();        

        IMyInventory Inventory = Container.GetInventory(0);
        Inventory.GetItems(InventotyItems);

        foreach (MyInventoryItem InventotyItem in InventotyItems)
        {
            result.Add(GetRusStr(InventotyItem.Type.SubtypeId) + ": " + InventotyItem.Amount.ToString());
        }
        return result;
    }

    /// <summary>
    /// Метод возвращает строку ошибки
    /// </summary>
    /// <param name="errNum">Номер ошибки</param>
    /// <returns></returns>
    public string ErrorStr(int errNum) {
        string result = "";
        switch (errNum)
        {
            case 1:
                result =  "Не найдено не одной LDC с тегом " + TAG_LCD + "! Добавьте тег в конец имени и перезагрузите скрипт";
                break;
            
        }
        return result;

    }

    /// <summary>
    /// Перевод строки
    /// </summary>
    /// <param name="str">Строка для перевода</param>
    /// <returns></returns>
    public string GetRusStr(string str){

        string result = "";

        switch (str)
        {
            //case "":
            //    result = "";
            //    break;
            default:
                result = str;
                break;
        }

        return result;
    }





    public void newVoid() { }

}