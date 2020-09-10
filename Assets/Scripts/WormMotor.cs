using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMotor : MonoBehaviour
{
    public Sprite head_North, head_South, head_East, head_West, body_NS, body_EW, body_NE, body_NW, body_SE, body_SW, tail_North, tail_South, tail_East, tail_West;
    public GameObject wormSegment;
    public int segments;

    private List<GameObject> seg = new List<GameObject>();

    public void BirthWorm(int x, int y)
    {
        GameObject tempGO;
        for(int i = 0; i < segments; i++)
        {
            tempGO = Instantiate(wormSegment, new Vector2(x + i, y), Quaternion.identity, transform);
            tempGO.GetComponent<WormSegmentController>().parentObject = this.gameObject;
            tempGO.GetComponent<WormSegmentController>().facing = WormSegmentController.direction.west;
            tempGO.GetComponent<WormSegmentController>().isHead = false; tempGO.GetComponent<WormSegmentController>().isTail = false; //set defualt body image and head/tail setting
            if (i == 0) { tempGO.GetComponent<WormSegmentController>().isHead = true; } //set segment to head
            if (i == (segments - 1)) { tempGO.GetComponent<WormSegmentController>().isTail = true; } //set segment to tail
            seg.Add(tempGO);
        }
        SetBodySprites();
    }

    public void GrowWorm()
    {
        GameObject tempGO; float x = seg[seg.Count - 1].transform.position.x, y = seg[seg.Count - 1].transform.position.y;
        tempGO = Instantiate(wormSegment, new Vector2(x,y), Quaternion.identity, transform);
        tempGO.GetComponent<WormSegmentController>().parentObject = this.gameObject;
        tempGO.GetComponent<WormSegmentController>().facing = seg[seg.Count-1].GetComponent<WormSegmentController>().facing;
        seg[seg.Count -1].GetComponent<WormSegmentController>().isTail = false;
        tempGO.GetComponent<WormSegmentController>().isTail = true;        
        seg.Add(tempGO);        
        for(int i = 0; i < seg.Count; i++)
        {
            seg[i].GetComponent<WormSegmentController>().MyNum = i;
        }
    }

    public void ShrinkWorm()
    {
        if(seg.Count > 3)
        {
            seg.Remove(seg[seg.Count - 1]);
            seg[seg.Count - 1].GetComponent<WormSegmentController>().isTail = true;
        }
    }

    private void SetBodySprites()
    {
        //Head Segment
        if (seg[0].GetComponent<WormSegmentController>().isHead && seg[0].GetComponent<WormSegmentController>().facing == WormSegmentController.direction.west) seg[0].GetComponent<SpriteRenderer>().sprite = head_West;
        if (seg[0].GetComponent<WormSegmentController>().isHead && seg[0].GetComponent<WormSegmentController>().facing == WormSegmentController.direction.east) seg[0].GetComponent<SpriteRenderer>().sprite = head_East;
        if (seg[0].GetComponent<WormSegmentController>().isHead && seg[0].GetComponent<WormSegmentController>().facing == WormSegmentController.direction.north) seg[0].GetComponent<SpriteRenderer>().sprite = head_North;
        if (seg[0].GetComponent<WormSegmentController>().isHead && seg[0].GetComponent<WormSegmentController>().facing == WormSegmentController.direction.south) seg[0].GetComponent<SpriteRenderer>().sprite = head_South;

        //Tail Segment
        if (seg[seg.Count - 2].transform.position.y == (seg[seg.Count - 1].transform.position.y + 1)) seg[seg.Count - 1].GetComponent<SpriteRenderer>().sprite = tail_North; 
        if (seg[seg.Count - 2].transform.position.y == (seg[seg.Count - 1].transform.position.y - 1)) seg[seg.Count - 1].GetComponent<SpriteRenderer>().sprite = tail_South; 
        if (seg[seg.Count - 2].transform.position.x == (seg[seg.Count - 1].transform.position.x + 1)) seg[seg.Count - 1].GetComponent<SpriteRenderer>().sprite = tail_East; 
        if (seg[seg.Count - 2].transform.position.x == (seg[seg.Count - 1].transform.position.x - 1)) seg[seg.Count - 1].GetComponent<SpriteRenderer>().sprite = tail_West; 

        for (int i = 1; i < seg.Count-1; i++)
        {
            //local scope helper variables
            int pX = (int)seg[i - 1].transform.position.x, nX = (int)seg[i + 1].transform.position.x, pY = (int)seg[i - 1].transform.position.y, nY = (int)seg[i + 1].transform.position.y, tX = (int)seg[i].transform.position.x, tY = (int)seg[i].transform.position.y;
            //body_NS, body_EW, body_NE, body_NW, body_SE, body_SW
            //If both seg-1 and seg+1 are either north or south, then NS            
            if (pX == tX && nX == tX) seg[i].GetComponent<SpriteRenderer>().sprite = body_NS;
            //If both seg-1 and seg+1 are either west or east, then EW
            if (pY == tY && nY == tY) seg[i].GetComponent<SpriteRenderer>().sprite = body_EW;

            //tail is north, head is east
            if(nX == tX && nY == tY + 1 && pX == tX + 1 && pY == tY) seg[i].GetComponent<SpriteRenderer>().sprite = body_NE;
            //tail is north, head is west
            if (nX == tX && nY == tY + 1 && pX == tX - 1 && pY == tY) seg[i].GetComponent<SpriteRenderer>().sprite = body_NW;
            //tail is south, head is east
            if (nX == tX && nY == tY - 1 && pX == tX + 1 && pY == tY) seg[i].GetComponent<SpriteRenderer>().sprite = body_SE;
            //tail is south, head is west
            if (nX == tX && nY == tY - 1 && pX == tX - 1 && pY == tY) seg[i].GetComponent<SpriteRenderer>().sprite = body_SW;

            //tail is east, head is north
            if (nX == tX + 1 && nY == tY && pX == tX && pY == tY + 1) seg[i].GetComponent<SpriteRenderer>().sprite = body_NE;
            //tail is east, head is south
            if (nX == tX + 1 && nY == tY && pX == tX && pY == tY - 1) seg[i].GetComponent<SpriteRenderer>().sprite = body_SE;
            //tail is west, head is north
            if (nX == tX - 1 && nY == tY && pX == tX && pY == tY + 1) seg[i].GetComponent<SpriteRenderer>().sprite = body_NW;
            //tail is west, head is south
            if (nX == tX - 1 && nY == tY && pX == tX && pY == tY - 1) seg[i].GetComponent<SpriteRenderer>().sprite = body_SW;
        }
    }

    public void MoveWorm()
    {
        //Determine which direction to move
        int north = 0, south=0, east=0, west=0, randomness=75, way2go = 0, highest = 0, testX = (int)seg[0].transform.position.x, testY = (int)seg[0].transform.position.y;

        //Worm prefers to move straight, but will also make 90degree turns
        if (seg[0].GetComponent<WormSegmentController>().facing == WormSegmentController.direction.north) { north += 100; east += 50; west += 50; }
        if (seg[0].GetComponent<WormSegmentController>().facing == WormSegmentController.direction.south) { south += 100; east += 50; west += 50; }
        if (seg[0].GetComponent<WormSegmentController>().facing == WormSegmentController.direction.east) { east += 100; north += 50; south += 50; }
        if (seg[0].GetComponent<WormSegmentController>().facing == WormSegmentController.direction.west) { west += 100; north += 50; south += 50; }

        //Worm will not run into rocks
        if (GameManager.GAME.tileAssignment[testX, testY + 1] == 1) { north += -1000; }
        if (GameManager.GAME.tileAssignment[testX, testY - 1] == 1) { south += -1000; }
        if (GameManager.GAME.tileAssignment[testX + 1, testY] == 1) { east += -1000; }
        if (GameManager.GAME.tileAssignment[testX - 1, testY] == 1) { west += -1000; }

        //Add a bit of randomness, for spice
        north += Random.Range(0, randomness);
        south += Random.Range(0, randomness);
        east += Random.Range(0, randomness);
        west += Random.Range(0, randomness);
               
        //Find the most desirable outcoume
        highest = 0;
        if (highest < north) { way2go = 0; highest = north; }
        if (highest < south) { way2go = 1; highest = south; }
        if (highest < east) { way2go = 2; highest = east; }
        if (highest < west) { way2go = 3; highest = west; }

        //Move segments, backwards
        for (int i = seg.Count - 1; i > 0; i--) seg[i].transform.position = seg[i - 1].transform.position;

        //Move Seg0
        if (way2go == 0) { seg[0].GetComponent<WormSegmentController>().facing = WormSegmentController.direction.north; seg[0].transform.position = new Vector2(seg[0].transform.position.x, seg[0].transform.position.y + 1); } //move Seg0 north
        if (way2go == 1) { seg[0].GetComponent<WormSegmentController>().facing = WormSegmentController.direction.south; seg[0].transform.position = new Vector2(seg[0].transform.position.x, seg[0].transform.position.y - 1); } //move Seg0 south
        if (way2go == 2) { seg[0].GetComponent<WormSegmentController>().facing = WormSegmentController.direction.east; seg[0].transform.position = new Vector2(seg[0].transform.position.x + 1, seg[0].transform.position.y); } //move Seg0 east
        if (way2go == 3) { seg[0].GetComponent<WormSegmentController>().facing = WormSegmentController.direction.west; seg[0].transform.position = new Vector2(seg[0].transform.position.x - 1, seg[0].transform.position.y); } //move Seg0 west

        //determine segment facing
        SetBodySprites();
    }

    public Vector2 HeadPos()
    {
        return seg[0].transform.position;
    }
}
