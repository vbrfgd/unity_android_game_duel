using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class LogicScript : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pauseButton;

    public Slider volumeSlider;
    public AudioSource hitSound;
    public AudioSource blockSound;
    public AudioSource parrySound;
    public AudioSource heavyHitSound;

    public float StrikeDuration = 0.5f;

    public float enemyDmg;
    public float enemyLimbDmg;
    public float enemyHealth;
    public float enemyMaxHealth;
    public float enemyHeadHealth;
    public float enemyTorsoHealth;
    public float enemyLeftArmHealth;
    public float enemyRightArmHealth;
    public Image enemyHealthBarImage;
    //public GameObject enemySword;
    public TextMeshProUGUI enemyHeathText;
    public float enemyPrepare = 2;
    public float enemyHit = 3;
   
    private float enemyHitTimer = 0;
    private float enemyDirectionValue = -1;
    private float enemyStrikeDurationTimer = 0;
    public float enemyStunDuration;

    public Image enemySwordUp;
    public Image enemySwordDown;
    public Image enemySwordLeft;
    public Image enemySwordRight;
    public Image enemyIdle;
    public Image enemySwordUpStrike;
    public Image enemySwordDownStrike;
    public Image enemySwordLeftStrike;
    public Image enemySwordRightStrike;
    //public Image enemySwordCenter;
    public Image enemyStun;

    public Image enemyHeadCrippled;
    public Image enemyTorsoCrippled;
    public Image enemyArmCrippled;

    public float playerDmg;
    public float playerLimbDmg;
    public float playerHealth;
    public float playerMaxHealth;
    public float playerHeadHealth=100f;
    public float playerTorsoHealth=100f;
    public float playerLeftArmHealth=100f;
    public float playerRightArmHealth=100f;
    private float playerChargeUp = 0f;
    public float playerChargeThreshold = 5f;
    private float playerDirectionValue = -1;
    private float lastPlayerDirectionValue = -1;
    public Image playerHealthBarImage;
    public TextMeshProUGUI playerHealthText;
    public GameObject playerShield;
    public GameObject playerShieldDown;
    public bool shieldRaised = false;
    public Image playerHourGlass;
    public Image playerChargeMeter;
    private float playerHitTimer;
    public float playerCoolDown = 1;
    private float playerStrikeDurationTimer = 0;
    public GameObject PlayerScreenFlash;

    public Image playerSwordIdle;
    public Image playerSwordUWU;
    public Image playerSwordUS;
    public Image playerSwordDWU;
    public Image playerSwordDS;
    public Image playerSwordLWU;
    public Image playerSwordLS;
    public Image playerSwordRWU;
    public Image playerSwordRS;

    public Image playerHead;
    public Image playerTorso;
    public Image playerLeftArm;
    public Image playerRightArm;

    public Image crossHair;
    public Image crossHairUp;
    public Image crossHairDown;
    public Image crossHairLeft;
    public Image crossHairRight;
    //public Image crossHairCenter;


    public bool gameInProgress = true;
    public GameObject gameOverWinScreen;
    public GameObject gameOverLoseScreen;

    public VJHandler jsMovement;

    private Vector3 direction;
    private Vector3 upDirection;

    public void pauseButton()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
        _pauseButton.SetActive(false);
    }

    public void resumeButton()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
        _pauseButton.SetActive(true);
    }

    public void EnemyFlash()
    {
        switch(enemyDirectionValue)
        {
            case 1:
                StartCoroutine(ImgShowAndHide(enemySwordUp, 0.2f));
                break;
            case 2:
                StartCoroutine(ImgShowAndHide(enemySwordRight, 0.2f));
                break;
            case 3:
                StartCoroutine(ImgShowAndHide(enemySwordDown, 0.2f));
                break;
            case 4:
                StartCoroutine(ImgShowAndHide(enemySwordLeft, 0.2f));
                break;
            default:
                StartCoroutine(ImgShowAndHide(enemyIdle, 0.2f));
                break;
        }
    }

    IEnumerator ShowAndHide(GameObject ScreenFlash, float delay)
    {
        ScreenFlash.SetActive(true);
        yield return new WaitForSeconds(delay);
        ScreenFlash.SetActive(false);
    }

    IEnumerator ImgShowAndHide(Image ImgFlash, float delay)
    {
        ImgFlash.color = new Color32(255,0,0,60);
        yield return new WaitForSeconds(delay);
        ImgFlash.color = new Color32(255,255,255,255);
    }

    [ContextMenu("Hit Enemy")]
    public void hitEnemy()
    {
        EnemyFlash();
        if ((playerChargeUp/playerChargeThreshold) > 0.5)
        {
            heavyHitSound.Play();
        }
        else
        {
            hitSound.Play();
        }
        enemyHealth = (float)(enemyHealth - playerDmg*(1+2*(playerChargeUp/playerChargeThreshold)));
        playerChargeUp = 0;
        //Debug.Log(enemyHealth.ToString());
        enemyHeathText.text = enemyHealth.ToString();
        enemyHealthBarImage.fillAmount = Mathf.Clamp((float)enemyHealth / enemyMaxHealth, 0, 1f);
    }

    [ContextMenu("Hit Player")]
    public void hitPlayer()
    {
        StartCoroutine(ShowAndHide(PlayerScreenFlash, 0.2f));
        hitSound.Play();
        playerChargeUp = 0;
        playerHealth = playerHealth - enemyDmg;
        switch (enemyDirectionValue)
        {
            case 1:
                if (playerHeadHealth > 0)
                {
                    playerHeadHealth -= enemyLimbDmg;
                }
                if (playerHeadHealth < 0)
                {
                    playerHeadHealth = 0;
                    playerHead.color = new Color32(255, 0, 0, 255);
                    playerCoolDown = playerCoolDown * 2;
                }
                break;
            case 2:
                if (playerRightArmHealth > 0)
                {
                    playerRightArmHealth -= enemyLimbDmg;
                }
                if (playerRightArmHealth < 0)
                {
                    playerRightArmHealth = 0;
                    playerRightArm.color = new Color32(255, 0, 0, 255);
                    playerDmg = playerDmg * 0.5f;
                }
                break;
            case 3:
                if (playerTorsoHealth > 0)
                {
                    playerTorsoHealth -= enemyLimbDmg;
                }
                if (playerTorsoHealth < 0)
                {
                    playerTorsoHealth = 0;
                    playerTorso.color = new Color32(255, 0, 0, 255);
                    enemyDmg = enemyDmg * 2;
                }
                break;
            default:
                if (playerLeftArmHealth > 0)
                {
                    playerLeftArmHealth -= enemyLimbDmg;
                }
                if (playerLeftArmHealth < 0)
                {
                    playerLeftArm.color = new Color32(255, 0, 0, 255);
                }
                break;

        }
        playerHealthText.text = playerHealth.ToString();
        playerHealthBarImage.fillAmount = Mathf.Clamp((float)playerHealth / playerMaxHealth, 0, 1f);
    }

    [ContextMenu("Win Game")]
    public void gameOverWin()
    {
        gameOverWinScreen.SetActive(true);
    }

    [ContextMenu("Lose Game")]
    public void gameOverLose()
    {
        gameOverLoseScreen.SetActive(true);
    }

    public void enemyBehavior()
    {
        //Debug.Log(strikeDurationTimer.ToString());
        if (enemyStrikeDurationTimer > 0)
        {
            enemyStrikeDurationTimer -= Time.deltaTime;
        }
        else
        {
            enemySwordUpStrike.enabled = false;
            enemySwordDownStrike.enabled = false;
            enemySwordLeftStrike.enabled = false;
            enemySwordRightStrike.enabled = false;
        }

        if (enemyHitTimer < 0)
        {
            enemyStun.enabled = true;
        }
        else
        {
            enemyStun.enabled = false;
        }

        if (enemyHitTimer < enemyPrepare && enemyStrikeDurationTimer <= 0)
        {
            enemyIdle.enabled = true;
        }


        if (enemyHitTimer < enemyPrepare)
        {
            enemyHitTimer += Time.deltaTime;
        }
        else if (enemyHitTimer > enemyPrepare && enemyHitTimer < enemyHit && gameInProgress)
        {
            enemyIdle.enabled = false;
            if (enemyDirectionValue == -1)
            {
                enemyDirectionValue = Random.Range(1, 5);
                //Debug.Log(enemyDirectionValue);
                if (enemyDirectionValue == 1)
                {
                    enemySwordUp.enabled = true;
                }
                else if (enemyDirectionValue == 2)
                {
                    enemySwordRight.enabled = true;
                }
                else if (enemyDirectionValue == 3)
                {
                    enemySwordDown.enabled = true;
                }
                else
                {
                    enemySwordLeft.enabled = true;
                }
                //else
                //{
                //    enemySwordCenter.enabled = true;
                //}
            }
            //enemySword.SetActive(true);
            enemyHitTimer += Time.deltaTime;
        }
        else if (enemyHitTimer > enemyHit && gameInProgress)
        {
            enemyHitTimer = 0;
            enemyStrikeDurationTimer = StrikeDuration;
            if (enemyDirectionValue == 1)
            {
                enemySwordUpStrike.enabled = true;
            }
            else if (enemyDirectionValue == 2)
            {
                enemySwordRightStrike.enabled = true;
            }
            else if (enemyDirectionValue == 3)
            {
                enemySwordDownStrike.enabled = true;
            }
            else
            {
                enemySwordLeftStrike.enabled = true;
            }

            if (shieldRaised == true)
            {
                blockSound.Play();
            }   
            else if (enemyDirectionValue == playerDirectionValue)
            {
                parrySound.Play();
                enemyHitTimer = -enemyStunDuration;
            }
            else
            {
                hitPlayer();
                
            }
            
            enemySwordUp.enabled = false;
            enemySwordDown.enabled = false;
            enemySwordLeft.enabled = false;
            enemySwordRight.enabled = false;
            //enemySwordCenter.enabled = false;
            //enemySword.SetActive(false);
            enemyDirectionValue = -1;
        }
    }

    public void playerRaiseShield()
    {
        if (playerHitTimer >= playerCoolDown && gameInProgress && playerLeftArmHealth >= 0)
        {
            playerShield.SetActive(true);
            playerShieldDown.SetActive(false);
            shieldRaised = true;
        }
        else
        {
            playerShield.SetActive(false);
            shieldRaised = false;
        }
    }

    public void playerLowerShield()
    {
        if (playerHitTimer >= playerCoolDown && gameInProgress)
        {
            playerShield.SetActive(false);
            playerShieldDown.SetActive(true);
            shieldRaised = false;
        }
    }

    public int playerRaiseSword()
    {
        crossHair.enabled = false;
        //crossHairCenter.enabled = false;
        crossHairUp.enabled = false;
        crossHairDown.enabled = false;
        crossHairLeft.enabled = false;
        crossHairRight.enabled = false;

        if (Input.GetKey(KeyCode.UpArrow) == true && gameInProgress && playerHitTimer >= playerCoolDown)
        {
            crossHairUp.enabled = true;
            return 1;
        }
        else if (Input.GetKey(KeyCode.DownArrow) == true && gameInProgress && playerHitTimer >= playerCoolDown)
        {
            crossHairDown.enabled = true;
            return 3;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) == true && gameInProgress && playerHitTimer >= playerCoolDown)
        {
            crossHairLeft.enabled = true;
            return 4;
        }
        else if (Input.GetKey(KeyCode.RightArrow) == true && gameInProgress && playerHitTimer >= playerCoolDown)
        {
            crossHairRight.enabled = true;
            return 2;
        }
        else if (Input.GetKey(KeyCode.Space) == true && gameInProgress && playerHitTimer >= playerCoolDown)
        {
            //crossHairCenter.enabled = true;
            return 5;
        }
        else if (gameInProgress)
        {
            crossHair.enabled = true;
            return 0;
        }
        else
        {
            return -1;
        }
    }

    public int joyStickPlayerRaiseSword()
    {
        crossHair.enabled = false;
        //crossHairCenter.enabled = false;
        crossHairUp.enabled = false;
        crossHairDown.enabled = false;
        crossHairLeft.enabled = false;
        crossHairRight.enabled = false;

        playerSwordUWU.enabled = false;
        playerSwordDWU.enabled = false;
        playerSwordLWU.enabled = false;
        playerSwordRWU.enabled = false;
        

        var directionArray = new float[] {direction.x, -direction.x, direction.y, -direction.y};

        //Debug.Log(direction.x.ToString());
        //Debug.Log(direction.y.ToString());


        if (gameInProgress)
        {
            if (direction.magnitude != 0)
            {
                playerSwordIdle.enabled = false;
             
                if (directionArray.Max() == direction.y && playerHitTimer >= playerCoolDown)
                {
                    crossHairUp.enabled = true;
                    playerSwordUWU.enabled = true;
                    return 1;
                }
                else if (directionArray.Max() == -direction.y && playerHitTimer >= playerCoolDown)
                {
                    crossHairDown.enabled = true;
                    playerSwordDWU.enabled = true;
                    return 3;
                }
                else if (directionArray.Max() == -direction.x && playerHitTimer >= playerCoolDown)
                {
                    crossHairLeft.enabled = true;
                    playerSwordLWU.enabled = true;
                    return 4;
                }
                else if (directionArray.Max() == direction.x && playerHitTimer >= playerCoolDown)
                {
                    crossHairRight.enabled = true;
                    playerSwordRWU.enabled = true;
                    return 2;
                }
                /*else if (Input.GetKey(KeyCode.Space) == true && playerHitTimer >= playerCoolDown)
                {
                    crossHairCenter.enabled = true;
                    return 5;
                }*/
                else
                {
                    crossHair.enabled = true;
                    playerSwordIdle.enabled = true;
                    return 0;
                }
            }
            else
            {
                crossHair.enabled = true;
                return 0;
            }
        }
        else
        {
            return -1;
        }
    }

    public void playerAttack()
    {
        //playerRaiseSword();
        playerDirectionValue = joyStickPlayerRaiseSword();
        
        if (playerStrikeDurationTimer > 0)
        {
            playerStrikeDurationTimer -= Time.deltaTime;
        }
        else
        {
            playerSwordUS.enabled = false;
            playerSwordDS.enabled = false;
            playerSwordLS.enabled = false;
            playerSwordRS.enabled = false;
        }

        if (playerHitTimer < playerCoolDown && playerStrikeDurationTimer <= 0)
        {
            playerSwordIdle.enabled = true;
        }


        var upDirectionArray = new float[] { upDirection.x, -upDirection.x, upDirection.y, -upDirection.y };

        if (gameInProgress && playerHitTimer >= playerCoolDown)
        {
            if (upDirection.magnitude != 0)
            {
                playerStrikeDurationTimer = StrikeDuration;
                if (upDirectionArray.Max() == upDirection.y)
                {
                    if (enemyDirectionValue != 1)
                    {
                        playerSwordUS.enabled = true;
                        hitEnemy();
                        if (enemyHeadHealth > 0)
                        {
                            enemyHeadHealth = (float)(enemyHeadHealth - playerLimbDmg * (1 + 2 * (playerChargeUp / playerChargeThreshold)));
                        }
                        if (enemyHeadHealth < 0)
                        {
                            enemyHeadHealth = 0;
                            enemyHeadCrippled.enabled = true;
                            enemyStunDuration = enemyStunDuration * 2;
                        }
                    }
                    playerHitTimer = 0;
                }
                else if (upDirectionArray.Max() == upDirection.x)
                {
                    if (enemyDirectionValue != 2)
                    {
                        playerSwordRS.enabled = true;
                        hitEnemy();
                        if (enemyRightArmHealth > 0)
                        {
                            enemyRightArmHealth = (float)(enemyRightArmHealth - playerLimbDmg * (1 + 2 * (playerChargeUp / playerChargeThreshold)));
                        }
                        if (enemyRightArmHealth < 0)
                        {
                            enemyRightArmHealth = 0;
                            if (enemyArmCrippled.enabled == false)
                            {
                                enemyArmCrippled.enabled = true;
                                enemyDmg = enemyDmg * 0.5f;
                            }
                        }
                    }
                    playerHitTimer = 0;
                }
                else if (upDirectionArray.Max() == -upDirection.y)
                {
                    if (enemyDirectionValue != 3)
                    {
                        playerSwordDS.enabled = true;
                        hitEnemy();
                        if (enemyTorsoHealth > 0)
                        {
                            enemyTorsoHealth = (float)(enemyTorsoHealth - playerLimbDmg * (1 + 2 * (playerChargeUp / playerChargeThreshold)));
                        }
                        if (enemyTorsoHealth < 0)
                        {
                            enemyTorsoHealth = 0;
                            enemyTorsoCrippled.enabled = true;
                            playerDmg = playerDmg * 2;
                        }
                    }
                    playerHitTimer = 0;
                }
                else if (upDirectionArray.Max() == -upDirection.x)
                {
                    if (enemyDirectionValue != 4)
                    {
                        playerSwordLS.enabled = true;
                        hitEnemy();
                        if (enemyLeftArmHealth > 0)
                        {
                            enemyLeftArmHealth = (float)(enemyLeftArmHealth - playerLimbDmg * (1 + 2 * (playerChargeUp / playerChargeThreshold)));
                        }
                        if (enemyLeftArmHealth < 0)
                        {
                            enemyLeftArmHealth = 0;
                            if (enemyArmCrippled.enabled == false)
                            {
                                enemyArmCrippled.enabled = true;
                                enemyDmg = enemyDmg * 0.5f;
                            }
                        }
                    }
                    playerHitTimer = 0;
                }
                /*else if (Input.GetKeyUp(KeyCode.Space) == true)
                {
                    if (enemyDirectionValue != 5)
                    {
                        hitEnemy();
                    }
                    playerHitTimer = 0;
                }*/
                jsMovement.PointerUpDirection = Vector3.zero;
            }
            
        }

        if (playerDirectionValue != lastPlayerDirectionValue)
        {
            lastPlayerDirectionValue = playerDirectionValue;
            playerChargeUp = 0;
        }
        else
        {
            if (playerDirectionValue != 0 && playerChargeUp < playerChargeThreshold)
            {
                playerChargeUp += Time.deltaTime;
            }
        }

        if (playerChargeUp > 0)
        {
            playerChargeMeter.enabled = true;
            playerChargeMeter.fillAmount = Mathf.Clamp(playerChargeUp / playerChargeThreshold, 0, 1f);
        }
        else
        {
            playerChargeMeter.enabled = false;
        }

        if (playerHitTimer < playerCoolDown)
        {
            playerHitTimer += Time.deltaTime;
            playerHourGlass.enabled = true;
            playerHourGlass.fillAmount = Mathf.Clamp(1 - playerHitTimer / playerCoolDown, 0, 1f);

        }
        else
        {
            playerHourGlass.enabled = false;
        }
    }

    public void restartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume");
        Time.timeScale = 1.0f;
        Application.targetFrameRate = 60;

        playerHeadHealth = playerMaxHealth;
        playerTorsoHealth = playerMaxHealth;
        playerLeftArmHealth = playerMaxHealth;
        playerRightArmHealth = playerMaxHealth;

        enemyHeadHealth = enemyMaxHealth;
        enemyTorsoHealth = enemyMaxHealth;
        enemyLeftArmHealth = enemyMaxHealth;
        enemyRightArmHealth = enemyMaxHealth;

        playerHitTimer = playerCoolDown;
        playerHourGlass.enabled = false;
        playerChargeMeter.enabled = false;
        PlayerScreenFlash.SetActive(false);

        crossHair.enabled = true;
        //crossHairCenter.enabled = false;
        crossHairUp.enabled = false;
        crossHairDown.enabled = false;
        crossHairLeft.enabled = false;
        crossHairRight.enabled = false;

        playerSwordUWU.enabled = false;
        playerSwordDWU.enabled = false;
        playerSwordLWU.enabled = false;
        playerSwordRWU.enabled = false;
        playerSwordUS.enabled = false;
        playerSwordDS.enabled = false;
        playerSwordLS.enabled = false;
        playerSwordRS.enabled = false;
        playerSwordIdle.enabled = true;

        enemySwordUp.enabled = false;
        enemySwordDown.enabled = false;
        enemySwordLeft.enabled = false;
        enemySwordRight.enabled = false;
        enemyIdle.enabled = false;
        //enemySwordCenter.enabled = false;
        enemyStun.enabled = false;

        enemyHeadCrippled.enabled = false;
        enemyTorsoCrippled.enabled = false;
        enemyArmCrippled.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");

        direction = jsMovement.InputDirection;
        upDirection = jsMovement.PointerUpDirection;

        enemyBehavior();
        //playerRaiseShield();
        playerAttack();

        //Debug.Log(upDirection.ToString());

        if (enemyHealth <= 0)
        {
            gameOverWin();
            gameInProgress = false;
        }

        if (playerHealth <= 0)
        {
            gameOverLose();
            gameInProgress = false;
        }
    }
}
