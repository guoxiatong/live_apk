using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageControl : MonoBehaviour
{
    public bool IsLive;
    private LivingRoomData[] LivingRooms;
    private VideoData[] videos;
    private int LeftIndex = 0;
    private int RightIndex = 0;
    private RectTransform content;
    private bool IsSlide = false;
    public RectTransform LeftPage;
    public RectTransform RightPage;
    public RectTransform CurrentPage;
    public void Init()
    {
        if (IsLive)
        {
            content = GetComponent<RectTransform>();
            StartCoroutine(DataClassInterface.IEGetDate(AllData.DataString + "/vr/getBroadcastList?pageSize=200", new DataClassInterface.OnDataGet<LivingRoomData[]>((LivingRoomData[] data, GameObject[] gos, string str) =>
              {
                  this.LivingRooms = data;
                  foreach (Transform t in transform)
                  {
                      InitPage(t.GetComponent<RectTransform>(), -1);
                      RightIndex += 4;
                  }
              }), null));
        }
        else
        {
            content = GetComponent<RectTransform>();
            StartCoroutine(DataClassInterface.IEGetDate(AllData.DataString + "/vr/getVideoList?pageSize=200", new DataClassInterface.OnDataGet<VideoData[]>((VideoData[] data, GameObject[] gos, string str) =>
              {
                  this.videos = data;
                  foreach (Transform t in transform)
                  {
                      InitPage(t.GetComponent<RectTransform>(), -1);
                      RightIndex += 4;
                  }

              }), null));
        }
    }

    //right==1向右,right==-1向左
    public void OnClick(int right)
    {
        if (IsSlide)
            return;
        if (right == 1)
        {
            LeftIndex -= 4;
            RightIndex -= 4;
            RightPage.anchoredPosition3D = LeftPage.anchoredPosition3D + Vector3.left * 300;
            RectTransform temp = RightPage;
            RightPage = CurrentPage;
            CurrentPage = LeftPage;
            LeftPage = temp;
            InitPage(LeftPage, right);
        }
        else
        {
            LeftIndex += 4;
            RightIndex += 4;
            LeftPage.anchoredPosition3D = RightPage.anchoredPosition3D + Vector3.right * 300;
            RectTransform temp = RightPage;
            RightPage = LeftPage;
            LeftPage = CurrentPage;
            CurrentPage = temp;
            InitPage(RightPage, right);
        }
        StartCoroutine(IESlide(content.anchoredPosition3D, right));
    }

    void InitPage(RectTransform page, int right)
    {
        if (IsLive)
        {
            for (int j = 0; j < 4; j++)
            {
                if (LivingRooms[j].id == GameObject.Find("Manager").GetComponent<MsgManager>().CurrentId)
                {
                    if (right == 1)
                    {
                        LeftIndex--;
                    }
                    else
                    {
                        RightIndex++;
                    }
                    break;
                }
            }
            int i = 0;
            foreach (RoomButtonControl button in page.GetComponentsInChildren<RoomButtonControl>())
            {
                LivingRoomData temp = LivingRooms[(((right == 1 ? LeftIndex : RightIndex) + i) + LivingRooms.Length) % LivingRooms.Length];
                if (temp.id == GameObject.Find("Manager").GetComponent<MsgManager>().CurrentId)
                {
                    i++;
                    temp = LivingRooms[(((right == 1 ? LeftIndex : RightIndex) + i) + LivingRooms.Length) % LivingRooms.Length];
                }
                button.Init2(temp.roomStatus == "1" ? VideoType.Live_On : VideoType.Live_Off, temp.id, temp.title, temp.coverImg1);
                button.name=temp.id.ToString();
                button.IntoLivingRoom();
                i++;
            }
        }
        else
        {
            for (int j = 0; j < 4; j++)
            {
                if (videos[j].workId == GameObject.Find("MsgCanvas").GetComponent<VideoManager>().Id)
                {
                    if (right == 1)
                    {
                        LeftIndex--;
                    }
                    else
                    {
                        RightIndex++;
                    }
                    break;
                }
            }
            int i = 0;
            foreach (RoomButtonControl button in page.GetComponentsInChildren<RoomButtonControl>())
            {
                VideoData temp = videos[(((right == 1 ? LeftIndex : RightIndex) + i) + videos.Length) % videos.Length];
                if (temp.workId == GameObject.Find("MsgCanvas").GetComponent<VideoManager>().Id)
                {
                    i++;
                    temp = videos[(((right == 1 ? LeftIndex : RightIndex) + i) + videos.Length) % videos.Length];
                }
                VideoType videoType = VideoType.Video2D;
                switch (temp.videoType)
                {
                    case 0:
                        videoType = VideoType.Video360;
                        break;
                    case 3:
                        videoType = VideoType.Video180;
                        break;
                    case 5:
                        videoType = VideoType.Video3D;
                        break;
                    case 6:
                        videoType = VideoType.Video2D;
                        break;
                    case 8:
                        videoType = VideoType.Video_Cube;
                        break;
                    default:
                        videoType = VideoType.Video2D;
                        Debug.LogError("新格式");
                        break;
                }
                button.Init2(videoType, temp.workId, temp.title, temp.cover);
                button.name=temp.workId.ToString();
                button.Into360Room();
                i++;
            }
        }
    }

    IEnumerator IESlide(Vector3 source, int right)
    {
        IsSlide = true;
        while (content.anchoredPosition3D != source + new Vector3(right * 300, 0, 0))
        {
            content.anchoredPosition3D = Vector3.Lerp(content.anchoredPosition3D, source + Vector3.right * right * 300, 0.1f);
            if (Vector3.Distance(content.anchoredPosition3D, source + Vector3.right * right * 300) < 1)
            {
                content.anchoredPosition3D = source + Vector3.right * right * 300;
                break;
            }
            yield return null;
        }
        IsSlide = false;
    }
}
