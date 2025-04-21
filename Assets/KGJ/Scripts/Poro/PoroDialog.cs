using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using Define;

public class PoroDialog : MonoBehaviour
{
    [Header("Text Offset")]
    [SerializeField] private Vector3 textOffset = new Vector3(0, 2, 0); // �����κ����� ������

    [Header("Dialogue Texts")]
    [SerializeField] List<string> shootDialogue;
    int index = 0;

    [SerializeField] float textInterval = 3f;

    private TextMeshPro dialogueText; // ��ȭ �ؽ�Ʈ
    private Transform enemy; // ���� Transform

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

        // ���� ��ġ�� �������� ���Ͽ� UI�� ��ġ�� ����
        Vector3 targetPosition = enemy.position + textOffset;
        transform.position = targetPosition;

        // UI�� ȸ���� �ʱ�ȭ�Ͽ� �׻� ������ �ٶ󺸰� ����
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }


    IEnumerator DialogueTalking()
    {
        dialogueText.color = Color.yellow;
        dialogueText.text = "��� ���~~!!!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�����. �ڳװ� �ٷ� �� PGA ����ΰ�?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�� �Գ�. ���� �ٷ� ���� ������ ����, ������� CEO�ϼ�.\n���� �����Ϸ� ������� �ߵ� �Ա���.";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "���� ���� ������ â����.\n�� ���� ��Ʈ�� ȭ��Ǻ��� �������.";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "������ �� ��û�̵��� �丮�� ���� ���� ������ �Ļ��� �����ϴ°� �ܿ� �ø����̶�ϱ�?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "��? ��¼�� ��ġ�Ǿ��İ�?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�׾� �� �׷������� �츮 ȸ��κ��� ���� ��� ���ؼ���!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "��û�ϱ�. ���� �׸� ȭ���� ������ ȥ���� ����Ű�� ������";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�츮 ����1�� ��������� ������ ������ ������ ū �⿩�� �ϰ� �ֳ�.";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�ڳ� ���޵� �� �� �ָӴϿ��� ������ �Ŷ� �̸��̾�~";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�׷��� �װ͵� �𸣰� CEO�� ��ġ��? �� ��� �͵� ������.";
        yield return new WaitForSeconds(1f);

        dialogueText.color = Color.yellow;
        dialogueText.text = "��? ����� ���� �źζ����� �� �׷��� �����״ٰ�?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�װ� �� �߸��� �ƴϾ�.\n�ڱ�� �ǰ� ���� �� �Ѱ� �� ������ �������?";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "��༭�� ����� �� �а� �������, ��. �� ����� ���� ��!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�츮 ������� �ƴϾ����� �� ����� �� ��������~\n���� ���� ����, �б�, ����, �ٸ��� �� ����?!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "������ ������ �������մϴ١���� ���ؾ� �ϴ� �� �Ƴ�?\n���� ���������� ��û�̰� �ʹ� ����!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�� �����? �׷� ���� ���� ������!\n�ֽ� �� �������� ���� ���� ���ֵ����� �����ؾ� �ϰŵ�!";
        yield return new WaitForSeconds(textInterval);

        _coroutine = null;

        // �÷��̾��� �̵��� ����
        player.CurrentState = PlayerState.Interaction;

        // TODO : �÷��̾��� ���� ����� ����

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
        dialogueText.text = "�� �̷��� ����? �� �񼭰� �� �����ڱ�. � �� ����!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "���塦 �� ������ ������ ������ ���� �� ��������.";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "�ڳ� ����, �� ���ݿ��� ������ �ž�. �˰� ������ ���� �� ������!";
        yield return new WaitForSeconds(textInterval);

        dialogueText.color = Color.yellow;
        dialogueText.text = "���� �︮���ͳ� �ҷ�. ���� �� �ִٰ� �� �ڰ� ��ھ�!";
        yield return new WaitForSeconds(textInterval);

        // TODO : ����� �α� ��Ŀ� �°� ���� �ʿ�
        _playerFire.CanFire = false;
        _saveEndingUI.TurnOn();
        Debug.Log("Save Ending");
        _coroutine = null;
    }
}
