using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class dedicated to control da behavior of the bar that is intended to graphically illustrate the
 * received values from the PLUX device.
 */
public class BarScript : MonoBehaviour {

    // Internal instance of the BarScript class.
    public static BarScript Instance;

    // Class global variables.
    private float fillAmount;
    public Image bar;
    private int maxValue = -1;

    /**
     * <summary>Awake is called when the script instance is being loaded.
     * Awake is used to initialize any variables or game state before the game starts. Awake is called only once during the lifetime of the script instance,
     * being always invoked before Start().</summary>
     * <see>https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html</see>
     */
    void Awake()
    {
        //Check if instance already exists
        if (Instance == null)
            //if not, set instance to this
            Instance = this;
    }

    /**
     * Set of instruction responsible for controlling the fraction of the bar that should be filled
     * accordingly to the value under analysis.
     */
    public int Value
    {
        set
        {
            // Initializing the bar value.
            if(value == -1)
            {
                fillAmount = 0;
                return;
            }

            // Baseline removal.
            int newValue = Mathf.Abs(value - 32767);

            // Dealing with out of bound values.
            maxValue = newValue > maxValue ? newValue : maxValue;

            // Define the new fillAmount value.
            fillAmount = newValue * 1f/maxValue;
        }
    }

    /**
     * <summary>Unity callback that is invoked when the script is enabled (is only executed one time).</summary>
     * <see>https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html</see>
     */
    void Start () {
		
	}

    /**
     * <summary>Unity callback called once per frame.</summary>
     * <see>https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html</see>
     */
    void Update () {
        HandleBar();
	}

    /**
     * <summary>Method responsible for setting the size of the bar parcel that should be filled (accordingly to the value
     * under analysis)</summary>
     */
    private void HandleBar()
    {
        // Update the bar only if the current value is different from the previous one.
        if(fillAmount == bar.fillAmount)
        {
            return;
        }

        // Update bar value.
        bar.fillAmount = fillAmount;
    }
}
