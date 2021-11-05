using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SiblingRuleTile : RuleTile<SiblingRuleTile.Neighbor> {
    public List<TileBase> siblings = new List<TileBase>();

    public class Neighbor : RuleTile.TilingRule.Neighbor
    {
        public const int Sibling = 3;
        public const int ThisOrSibling = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile)
    {
        switch (neighbor)
        {
            case Neighbor.Sibling: return siblings.Contains(tile);
            case Neighbor.ThisOrSibling: return tile == this || siblings.Contains(tile);
        }
        return base.RuleMatch(neighbor, tile);
    }
}