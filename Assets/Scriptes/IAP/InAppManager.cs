using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class InAppManager : MonoBehaviour, IStoreListener
{
    [SerializeField] private Booster[] _money;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public const string pMoney500 = "cash_500";
    public const string pMoney1500 = "cash_1500";
    public const string pMoney5000 = "cash_5000";
    public const string pMoney15000 = "cash_15000";
    public const string pMoney25000 = "cash_25000";

    public const string pMoney500AppStore = "shop_cash_500";
    public const string pMoney1500AppStore = "shop_cash_1500";
    public const string pMoney5000AppStore = "shop_cash_5000";
    public const string pMoney15000AppStore = "shop_cash_15000";
    public const string pMoney25000AppStore = "shop_cash_25000";

    public const string pMoney500GooglePlay = "shop_cash_500";
    public const string pMoney1500GooglePlay = "shop_cash_1500";
    public const string pMoney5000GooglePlay = "shop_cash_5000";
    public const string pMoney15000GooglePlay = "shop_cash_15000";
    public const string pMoney25000GooglePlay = "shop_cash_25000";

    private string currentIds;
    private PlayerScoreCounter _scoreCounter;

    void Start()
    {
        if (m_StoreController == null)
        {
            InitializePurchasing();
        }

        if (_scoreCounter == null)
            _scoreCounter = GetComponent<PlayerScoreCounter>();
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(pMoney500, ProductType.Consumable, new IDs() { { pMoney500AppStore, AppleAppStore.Name }, { pMoney500GooglePlay, GooglePlay.Name } });
        builder.AddProduct(pMoney1500, ProductType.Consumable, new IDs() { { pMoney1500AppStore, AppleAppStore.Name }, { pMoney1500AppStore, GooglePlay.Name } });
        builder.AddProduct(pMoney5000, ProductType.Consumable, new IDs() { { pMoney5000AppStore, AppleAppStore.Name }, { pMoney5000GooglePlay, GooglePlay.Name } });
        builder.AddProduct(pMoney15000, ProductType.Consumable, new IDs() { { pMoney15000AppStore, AppleAppStore.Name }, { pMoney15000GooglePlay, GooglePlay.Name } });
        builder.AddProduct(pMoney25000, ProductType.Consumable, new IDs() { { pMoney25000AppStore, AppleAppStore.Name }, { pMoney25000GooglePlay, GooglePlay.Name } });


        UnityPurchasing.Initialize(this, builder);
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string productId)
    {
        currentIds = productId;

        try
        {
            if (IsInitialized())
            {
                Product product = m_StoreController.products.WithID(productId);

                if (product != null && product.availableToPurchase)
                {
                    Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.
                    m_StoreController.InitiatePurchase(product);
                }
                else
                {
                    Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                }
            }
            else
            {
                Debug.Log("BuyProductID FAIL. Not initialized.");
            }
        }
        catch (Exception e)
        {
            Debug.Log("BuyProductID: FAIL. Exception during purchase. " + e);
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) =>
            {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: Completed!");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        if (String.Equals(args.purchasedProduct.definition.id, currentIds, StringComparison.Ordinal))
        {
            var booster = _money.Where(a => a.ItemIOSId == currentIds).FirstOrDefault();
            if (booster != null)
                _scoreCounter.AddMoney((int)booster.Value);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
}
