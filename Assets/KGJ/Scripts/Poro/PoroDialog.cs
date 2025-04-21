using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Define;

public class PoroDialog : MonoBehaviour
{
    [Header("Text Offset")]
    [SerializeField] private Vector3 textOffset = new Vector3(0, 2, 0); // 적으로부터의 오프셋

    [Header("Dialogue Texts")]
    [SerializeField] List<string> shootDialogue;
    int index = 0;

    [SerializeField] float textInterval = 3f;

    private TextMeshPro dialogueText; // 대화 텍스트
    private Transform enemy; // 적의 Transform

    Coroutine _coroutine;

    PlayerController player;
    PlayerFire _playerFire;

    UI_SaveEnding _saveEndingUI;

    private void Awake()
    {
        dialogueText = GetComponent<TextMeshPro>();
    }

    private void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        _playerFire = FindAnyObjectByType<PlayerFire>();
        _saveEndingUI = FindAnyObjectByType<UI_SaveEnding>();
    }

    private void LateUpdate()
    {
        UpdateUIPosition();
    }

    public void StartEnding()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(DialogueTalking());
    }

    public void StartSavePoro()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(SavePoro());
    }

    public void Shoot()
    {
        dialogueText.color = Color.red;
        dialogueText.text = shootDialogue[index];
        index++;
    }

    public void UpdateUIPosition()
    {
        if (enemy == null) return;

        // 적의 위치에 오프셋을 더하여 UI의 위치를 설정
        Vector3 targetPosition = enemy.position + textOffset;
        transform.position = targetPosition;

        // UI의 회전을 초기화하여 항상 정면을 바라보게 설정
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    IEnumerator DialogueTalking()
    {
        dialogueText.color = Color.yellow;
        dialogueText.text = "사람 살려~~!!!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "…잠깐. 자네가 바로 그 PGA 요원인가?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "잘 왔네. 내가 바로 국가 경제의 심장, 희망생명 CEO일세.\n나를 구출하러 여기까지 잘도 왔구먼.";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "여긴 정말 더러운 창고지.\n내 개인 제트기 화장실보다 형편없어.";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "심지어 이 멍청이들은 요리를 할줄 몰라서 나한테 식사라고 제공하는게 겨우 시리얼이라니까?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "응? 어쩌다 납치되었냐고?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "그야 저 테러범들이 우리 회사로부터 돈을 뜯기 위해서지!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "멍청하긴. 뭐가 그리 화나서 국가적 혼란을 일으키는 건지…";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "우리 시총1위 희망생명은 오히려 국가의 발전에 큰 기여를 하고 있네.";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "자네 월급도 다 내 주머니에서 나오는 거라 이말이야~";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "그런데 그것도 모르고 CEO를 납치해? 못 배운 것들 같으니.";
        yield return new WaitForSeconds(1f);

        dialogueText.color = Color.yellow;
        dialogueText.text = "뭐? 보험금 지급 거부때문에 이 테러를 일으켰다고?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "그건 내 잘못이 아니야.\n자기들 건강 관리 못 한걸 왜 나한테 뒤집어씌워?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "계약서도 제대로 안 읽고 성내기는, 쯧. 난 법대로 했을 뿐!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "우리 희망생명 아니었으면 이 나라는 안 굴러갔어~\n내가 세운 병원, 학교, 빌딩, 다리가 몇 갠데?!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "나한테 오히려 ‘감사합니다’라고 말해야 하는 거 아냐?\n세상엔 배은망덕한 멍청이가 너무 많아!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "다 들었나? 그럼 빨리 나를 구출해!\n주식 더 떨어지기 전에 빨리 주주들한테 설명해야 하거든!";
        yield return new WaitForSeconds(textInterval);

        _coroutine = null;

        // 플레이어의 이동을 막음
        player.CurrentState = PlayerState.Interaction;

        // TODO : 플레이어의 무기 사용을 막음

        Vector3 startPos = player.transform.position;
        Vector3 targetPos = transform.position;
        float duration = 3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            player.transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPos;

        StartSavePoro();
    }

    IEnumerator SavePoro()
    {
        dialogueText.color = Color.yellow;
        dialogueText.text = "왜 이렇게 느려? 내 비서가 더 빠르겠군. 어서 날 꺼내!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "젠장… 이 따위로 밧줄을 묶으니 옷이 다 망가지지.";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "자네 월급, 내 세금에서 나오는 거야. 알고 있으면 빨리 일 시작해!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "빨리 헬리콥터나 불러. 여기 더 있다간 내 코가 썩겠어!";
        yield return new WaitForSeconds(textInterval);

        // TODO : 디버그 로그 양식에 맞게 수정 필요
        _playerFire.CanFire = false;
        _saveEndingUI.TurnOn();
        Debug.Log("Save Ending");
        _coroutine = null;
    }
}
