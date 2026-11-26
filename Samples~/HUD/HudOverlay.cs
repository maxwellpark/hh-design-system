using HootonHeroics.DesignSystem;
using UnityEngine;
using UnityEngine.UIElements;

/// A game HUD built from the design system; put it on a GameObject with a UIDocument whose panel uses hh-theme.tss.
/// Its own buttons stand in for gameplay, so it runs under either input system.
[RequireComponent(typeof(UIDocument))]
public class HudOverlay : MonoBehaviour
{
    private const int MaxHealth = 100;
    private const int MaxLives = 3;

    private HHProgressBar health;
    private HHSegmentedBar lives;
    private HHBadge bagBadge;
    private HHToastStack toasts;
    private int hp = MaxHealth;
    private int livesLeft = MaxLives;
    private int newItems;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        // The sample owns the whole document; re-enabling this component alone keeps its root, so start it empty
        root.Clear();
        root.style.flexGrow = 1;
        root.style.justifyContent = Justify.SpaceBetween;
        root.style.paddingTop = 24;
        root.style.paddingLeft = 24;
        root.style.paddingRight = 24;
        root.style.paddingBottom = 24;
        root.Add(BuildTopBar());
        root.Add(BuildDebugRow());
        toasts = new HHToastStack();
        root.Add(toasts);
        Refresh();
    }

    private VisualElement BuildTopBar()
    {
        var bar = Row(Justify.SpaceBetween);
        var vitals = new VisualElement();
        vitals.style.width = 260;
        health = new HHProgressBar { title = "Health", lowValue = 0, highValue = MaxHealth, Tone = HHTone.Pink };
        lives = new HHSegmentedBar { Segments = MaxLives, Tone = HHTone.Peach };
        vitals.Add(health);
        vitals.Add(lives);
        bar.Add(vitals);

        var bag = new HHButton(OpenBag) { text = "Bag", Ghost = true };
        bagBadge = new HHBadge { Tone = HHTone.Teal };
        bagBadge.style.position = Position.Absolute;
        bagBadge.style.top = -6;
        bagBadge.style.right = -6;
        bag.Add(bagBadge);
        bar.Add(bag);
        return bar;
    }

    private VisualElement BuildDebugRow()
    {
        var row = Row(Justify.FlexStart);
        row.Add(new HHButton(TakeHit) { text = "Take a hit" });
        row.Add(new HHButton(Heal) { text = "Heal", Ghost = true });
        row.Add(new HHButton(PickUp) { text = "Pick up", Ghost = true });
        return row;
    }

    private static VisualElement Row(Justify justify)
    {
        var row = new VisualElement();
        row.style.flexDirection = FlexDirection.Row;
        row.style.justifyContent = justify;
        row.style.alignItems = Align.FlexStart;
        return row;
    }

    private void TakeHit()
    {
        hp -= 35;
        if (hp <= 0)
        {
            hp = MaxHealth;
            livesLeft -= 1;
            if (livesLeft == 0)
            {
                livesLeft = MaxLives;
                toasts.Show("Game over: lives restored", HHToastTone.Danger);
            }
            else
            {
                toasts.Show("Life lost", HHToastTone.Warning);
            }
        }
        Refresh();
    }

    private void Heal()
    {
        hp = Mathf.Min(MaxHealth, hp + 20);
        Refresh();
    }

    private void PickUp()
    {
        newItems += 1;
        toasts.Show("Picked up a Northwind map", HHToastTone.Success);
        Refresh();
    }

    private void OpenBag()
    {
        newItems = 0;
        Refresh();
    }

    private void Refresh()
    {
        health.value = hp;
        lives.Filled = livesLeft;
        bagBadge.Count = newItems;
        bagBadge.style.display = newItems > 0 ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
