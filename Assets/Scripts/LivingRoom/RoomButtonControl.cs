using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
public enum VideoType
{
    Video2D,
    Video3D,
    Video180,
    Video360,
    Video_Cube,
    Live_On,
    Live_Off
}
public class RoomButtonControl : MonoBehaviour
{
    public Color OnLive;
    public Color OffLive;
    public Color V2D;
    public Color V3D;
    public Color V180;
    public Color V360;
    public Color Cube;
    public Image Photo;
    public VideoType VType;
    public Transform LeftTag;
    public Transform RightTag;
    public Text VName;
    public string vname;
    public int ID;
    public string photo;
    private bool IsBigger = false;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        EventTrigger trigger;
        if ((trigger = GetComponent<EventTrigger>()) == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(new UnityAction<BaseEventData>(OnEnter));
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerExit;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(new UnityAction<BaseEventData>(OnExit));
        trigger.triggers.Add(entry);
    }

    void OnEnter(BaseEventData data)
    {
        StartCoroutine(IEBigger());
    }
    void OnExit(BaseEventData data)
    {
        StartCoroutine(IESmaller());
    }

    IEnumerator IEBigger()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        IsBigger = true;
        while (IsBigger && rectTransform.localScale.x < 1.2f)
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, Vector3.one * 1.2f, 0.15f);
            if (Mathf.Abs(rectTransform.localScale.x - 1.2f) <= 0.01f)
            {
                rectTransform.localScale = Vector3.one * 1.2f;
            }
            yield return null;
        }
        yield break;
    }

    IEnumerator IESmaller()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        IsBigger = false;
        while (!IsBigger && rectTransform.localScale.x > 1f)
        {
            rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, Vector3.one, 0.2f);
            if (Mathf.Abs(rectTransform.localScale.x - 1f) <= 0.01f)
            {
                rectTransform.localScale = Vector3.one;
            }
            yield return null;
        }
    }

    public void Init(VideoType VType, int id, string vName, string photo)
    {

        // Controller controller = new Controller();
        //LeftTag.gameObject.SetActive(false);
        //RightTag.gameObject.SetActive(false);
        vname = vName;
        ID = id;
        this.VType = VType;
        //StartCoroutine(DataClassInterface.IEGetSprite(photo, (Sprite sprite, GameObject gtb, string nothing) => { Photo.sprite = sprite; }, null));
        //直播间
        /*if (VType == VideoType.Live_Off || VType == VideoType.Live_On)
        {
          
            LeftTag.gameObject.SetActive(true);
            GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject.Find("EventController").GetComponent<Controller>().EnterLivingRoom();
               controller.living_room .GetComponentInChildren<MsgManager>().CurrentId = ID;
            });
            if (VType == VideoType.Live_On)
            {
                LeftTag.GetComponentInChildren<Text>().text = "直播中";
                LeftTag.GetComponent<Image>().color = OnLive;
            }
            if (VType == VideoType.Live_Off)
            {
                LeftTag.GetComponentInChildren<Text>().text = "未开播";
                LeftTag.GetComponent<Image>().color = OffLive;
            }
        }
        //视频
        else
        {
            RightTag.gameObject.SetActive(true);
            GetComponent<Button>().onClick.AddListener(() =>
            {
                Debug.Log("点击了");
                GameObject.Find("EventController").GetComponent<Controller>().Enter360DegreeVideos();
                controller._360_living_room.GetComponentInChildren<VideoManager>().Id = ID;
            });
            switch (VType)
            {
                case VideoType.Video2D:
                RightTag.GetComponentInChildren<Text>().text = "2D";
                RightTag.GetComponent<Image>().color = V2D;
                    break;
                case VideoType.Video3D:
                RightTag.GetComponentInChildren<Text>().text = "3D";
                RightTag.GetComponent<Image>().color = V3D;
                    break;
                case VideoType.Video180:
                RightTag.GetComponentInChildren<Text>().text = "180";
                RightTag.GetComponent<Image>().color = V180;
                    break;
                case VideoType.Video360:
                RightTag.GetComponentInChildren<Text>().text = "360";
                RightTag.GetComponent<Image>().color = V360;
                    break;
                default:
                Debug.LogError("VType有错误");
                    break;

            }
        }*/
    }

    public void Init2(VideoType VType, int id, string vName, string photo)
    {
        LeftTag.gameObject.SetActive(false);
        RightTag.gameObject.SetActive(false);
        vname = vName;
        ID = id;
        this.VType = VType;
        VName.text = vname;
        StartCoroutine(DataClassInterface.IEGetSprite(photo, (Sprite sprite, GameObject gtb, string nothing) => { Photo.sprite = sprite; }, null));
        //直播间
        if (VType == VideoType.Live_Off || VType == VideoType.Live_On)
        {
            LeftTag.gameObject.SetActive(true);
            if (VType == VideoType.Live_On)
            {
                LeftTag.GetComponentInChildren<Text>().text = "直播中";
                LeftTag.GetComponent<Image>().color = OnLive;
            }
            if (VType == VideoType.Live_Off)
            {
                LeftTag.GetComponentInChildren<Text>().text = "未开播";
                LeftTag.GetComponent<Image>().color = OffLive;
            }
        }
        //视频
        else
        {
            RightTag.gameObject.SetActive(true);
            switch (VType)
            {
                case VideoType.Video2D:
                    RightTag.GetComponentInChildren<Text>().text = "2D";
                    RightTag.GetComponent<Image>().color = V2D;
                    break;
                case VideoType.Video3D:
                    RightTag.GetComponentInChildren<Text>().text = "3D";
                    RightTag.GetComponent<Image>().color = V3D;
                    break;
                case VideoType.Video180:
                    RightTag.GetComponentInChildren<Text>().text = "180";
                    RightTag.GetComponent<Image>().color = V180;
                    break;
                case VideoType.Video360:
                    RightTag.GetComponentInChildren<Text>().text = "360";
                    RightTag.GetComponent<Image>().color = V360;
                    break;
                case VideoType.Video_Cube:
                    RightTag.GetComponentInChildren<Text>().text = "海外";
                    RightTag.GetComponent<Image>().color = Cube;
                    break;
                default:
                    Debug.LogError("VType有错误");
                    break;

            }
        }
    }

    public RoomButtonControl(VideoType VType, int id, string vName)
    {
        vname = vName;
        ID = id;
        this.VType = VType;
    }

    public void Setphoto(string photo)
    {
        this.photo = photo;
    }

    public void IntoLivingRoom()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
                    {
                       // Debug.Log("添加方法" + i + "画布名称" + panel.name + "名称·1" + roominfo[i + index * 9].vname + "视频：" + roominfo[i + index * 9].ID + "视频分类" + roominfo[i + index * 9].VType);

                        GameObject.Find("EventController").GetComponent<Controller>().EnterLivingRoom();
                        if(GameObject.Find("Living room"))
                        {
                            GameObject.Find("Living room").GetComponentInChildren<MsgManager>().CurrentId = Int32.Parse(name);
                        }
                      else  if (GameObject.Find("Living room(Clone)"))
                        {
                            Debug.Log("进入直播间");
                            GameObject.Find("Living room(Clone)").GetComponentInChildren<MsgManager>().CurrentId = Int32.Parse(name);
                        }
                    });
    }
    public void Into360Room()
    {
       
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
                    {
                       // Debug.Log("添加方法" + i + "画布名称" + panel.name + "名称·1" + roominfo[i + index * 9].vname + "视频：" + roominfo[i + index * 9].ID + "视频分类" + roominfo[i + index * 9].VType);

                        GameObject.Find("EventController").GetComponent<Controller>().Enter360DegreeVideos();
                        if(GameObject.Find("Three hundred and sixty dergee living room"))
                        {
                            GameObject.Find("Three hundred and sixty dergee living room").GetComponentInChildren<VideoManager>().Id = Int32.Parse(name);
                        }
                      else  if (GameObject.Find("Three hundred and sixty dergee living room(Clone)"))
                        {
                            Debug.Log("进入直播间");
                            GameObject.Find("Three hundred and sixty dergee living room(Clone)").GetComponentInChildren<VideoManager>().Id = Int32.Parse(name);
                        }
                    });
            }
}
