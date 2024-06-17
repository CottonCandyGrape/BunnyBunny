<!--
<p align="center">
    <img src="https://github.com/JeongHo16/BunnyBunny/assets/31890732/2dc2a1b0-88b8-4a0d-b4c6-f22faf98af6b" style="width:70%;"/>
</p>
-->
<h1>BunnyBunny</h1>

- ### 사용 엔진
- Unity 2022.3.15f1
- ### 개발 기간
- 2024.02 ~ 2024.05
- ### [Download the latest version .apk](https://github.com/JeongHo16/CottonCandyGrapes/releases/download/v1.1.1/Bunnyz_240612.apk)
- ### [YouTube Video](https://youtu.be/ccTnCZxCkPs)
#

## 기술소개서

<blockquote>  
  <details>
   <summary><b> &nbsp Class와 Json을 이용한 정보 저장 </b></summary>
   
</details>
</blockquote>
   <p>
       <a href="https://github.com/JeongHo16/BunnyBunny/blob/main/Assets/_Scripts/UserInfo.cs">UserInfo.cs</a>
    유저 정보 관리 json 으로
    인게임과 json 연동 경험치, 골드
  </p> 
#
<blockquote>  
  <details>
   <summary><b> &nbsp 오브젝트 풀링을 이용하여 메모리 관리하기 </summary>
     <p>
      - 몬스터 스폰
      - 총알 스폰
      - 사운드 
      로켓
      오브젝트 1개 on, off로 구현
    </p>    
  </details>
</blockquote>

#
<blockquote>  
  <details>
    <summary><b> &nbsp Scene이 바뀌어도 사라지지 않는 Singleton </summary>
     <p>
      AllSceneMgr
    </p>    
  </details>
</blockquote>

#
<blockquote>  
  <details>
   <summary><b> &nbsp 비동기로 Scene 전환 애니메이션 만들기 </summary>
     <p>
      이 섹션은 펼쳐질 때 보이는 내용입니다.
    </p>    
  </details>
</blockquote>

#
<blockquote>  
  <details>
   <summary><b> &nbsp 랜덤으로 스킬 나타내기 </summary>
     <p>
      스킬업 버튼
      랜덤으로 뜨기
    </p>    
  </details>
</blockquote>

#
<blockquote>  
  <details>
    <summary><b> &nbsp 중복되는 Scene 부분 재활용하기 </summary>
     <p>
      - UI
      기준 해상도 맞추고 가면 앵커 맞출 필요 없음
      UpLowUI가 반복되니깐 InGame 씬 제외하고 additive로 부르기
    </p>    
  </details>
</blockquote>

#
<blockquote>  
  <details>
   <summary><b> &nbsp 효율적인 충돌처리를 위한 Layer 설정 </summary>
     <p>      
      충돌 Layer 설정
      총알 - 몬스터
      몬스터 - 플레이어
    </p>    
  </details>
</blockquote>

#
<blockquote>  
  <details>
   <summary><b> &nbsp 상속을 이용한 무기 시스템 </summary>
     <p>
      3성 달성 후 1번 더 먹으면 진화
      Weapon 만들어 상속 하기
      WeaponMgr
    </p>    
  </details>
</blockquote>

#
<blockquote>  
  <details>
   <summary><b> &nbsp 화면 좌표를 이용한 드릴 바운딩 구현 </summary>
     <p>
      세로맵에서 못나가게 하는건 화면 좌표 아닌가? 
      화면 좌표 이용하는곳 찾아보기
    </p>    
  </details>
</blockquote>
