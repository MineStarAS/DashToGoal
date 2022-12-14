## 프로젝트 명 : Dash to Goal (DTG?)

---

### + 프로젝트 설명

> 여러 특색의 `플레이어` 중 하나를 선택해 `플레이 맵`에서 결승점에 골인 하는 게임.
>
> 게임 방식은 `스토리 모드(싱글 플레이)`와 `멀티 플레이`로 나눠지며,  
> `스토리 모드`를 우선적으로 제작할 예정.

---

### 플레이어 (Player)
- [상세 설명 링크](detail/player/Player.md)

> 모든 `플레이어`는 공통적으로 `좌우 이동`, `점프`, `아이템 사용`을 갖고 있으며  
> 각기 다른 1개 또는 2개의 `패시브 스킬`, 2개의 `액티브 스킬`을 보유 합니다.

> :: TODO :: (추가 고려)
> #### 액세서리 (Accessory)
> - 게임 시작 전 `User`가 임의로 액세서리 하나를 선택.
> - 악세서리는 패시브 스킬을 갖고있음.

#### 점프 (Jump)

> `Y축`으로만 `Vector`를 부여하며 `플레이어`가 받고 있는 `효과`에 따라  
> `Vector`값 또는 `공중 점프 횟수`가 변할 수 있습니다.

#### 스킬 (Skill)

> #### 패시브 스킬 (Passive Skill)
> `패시브 스킬`은 `일반형`, `조건형`, `지형 특화`로 나눠집니다.  
> `플레이어`당 1개 또는 2개의 `패시브 스킬`을 갖습니다.
>
> > #### 일반형 (Normal Type)
> > 아무런 조건 없이 항상 `플레이어`에게 임의의 효과를 부여합니다.
>
> > #### 조건형 (Conditional Type)
> > `조건 패시브`에 맞는 `Event`가 발생 했을 때  
> > `플레이어`에게 임의의 효과를 부여합니다.
> > 조건 예시
> > - 5M 이내에서 다른 `플레이어`가 `액티브 스킬`을 사용 하였을 때
> > - `아이템`을 얻을 때
> > - `아이템`을 보유하고 있지 않을 때
>
> > #### 지형 특화 (Terrain Type) (`조건형`의 하위 개념)
> > `플레이어`가 밟고 있는 `맵 타일`의 `지형`과 지형 특화의 `지형`이 같은 동안  
> > `플레이어`에게 임의의 효과를 부여합니다.

> #### 액티브 스킬 (Active Skill)
> `액티브 스킬`은 `일반형`, `조건형`, `충전형`으로 나눠집니다.  
> `플레이어`당 2개의 `액티브 스킬`을 갖습니다.
>
> > #### 일반형 (Normal Type)
> > `쿨타임`만이 활성화 상태에 영향을 끼칩니다.
>
> > #### 조건형 (Conditional Type)
> > `쿨타임`이 있을 수도, 없을 수도 있습니다.
> >
> > 조건 예시
> > - 5M 이내에 다른 `플레이어`가 있을 경우
> > - 6M 이내에 조건에 맞는 `상호작용 오브젝트`가 있을 경우
>
> > #### 충전형 (Charge Type) (`조건형`의 하위 개념)
> > `쿨타임`은 없는게 기본이지만  
> > `플레이어`의 컨셉 또는 `밸런스` 문제로 `쿨타임`이 있을 수 있습니다.   
> > `Int`형태의 게이지 `필드`가 필요합니다.
> >
> > 조건 예시
> > - `아이템`을 얻을 때, 충전 +1
> > - `점프`를 할 때, 충전 +1
> > - `부정적인 효과`를 부여받을 때, 충전 -1
> > - `부정적인 효과`인 `기절`를 부여받을 때, 충전 초기화

--- 

### 플레이 맵 (Play Map)

> 게임이 진행 되는 공간이며 각종 `오브젝트`가 배치되어 있습니다.

#### 오브젝트 (Object)

> `플레이 맵`에 `임의` 또는 `무작위` 위치에 배치되는 물체들을 칭합니다.
> 
> > #### 맵 타일 (Map Tile)
> > `맵 타일`은 `플레이어`가 충돌하는 오브젝트이며,  
> > `바닥`과 `벽` 등을 만들 때 사용됩니다.
> >
> > `지형`이라는 값을 `Set<지형>`형태로 가지고 있으며 `플레이어`가 밟고있을 시  
> > `플레이어`가 보유한 `지형 특화 패시브`에 영향을 줄 수 있습니다.
> > > #### 지형 (Terrain)
> > > `열거형` 클래스이며 '맵 타일'의 환경 뿐만 아니라 `경사로`, `절벽`등으로도  
> > > 구분하여 사용 됩니다.
>
> > #### 마나 심볼 (Mana Symbol)
> > `플레이어`와 충돌 하지 않으며 `플레이어`의 `스킬`과 상호작용되어 사용됩니다.
>
> > #### 아이템 심볼 (Item Symbol)
> > `임의` 또는 `무작위`의 얻을 수 있는 구 형태의 오브젝트입니다.  
> > `Set<아이템>`의 값을 가지며 해당 값 중 하나를 `무작위`로 얻습니다.  
> > `Set<아이템>`이 비어있을 경우 `아이템 심볼`이 `생성`되지 않습니다.
>
> > #### 장치 (Device)
> > `플레이어`가 `접촉` 또는 `충돌`시 `플레이어`에게 `효과` 또는 `Vector`를 부여하는 오브젝트입니다.

#### 포인트 (Point)

> `플레이 맵`에 `임의`의 위치를 지정하는 객체입니다.
>
> > #### 아이템 심볼 포인트 (Item Symbol Point)
> > `아이템 심볼`이 `생성`되는 포인트입니다.  
> > `Set<아이템>`의 값을 가지며 해당 값을 `생성`하는 `아이템 심볼`에 `입력`됩니다.

#### 구역 (Area)

> `플레이 맵`에 `임의`의 공간을 지정하는 객체입니다.
> 두 좌표 값을 `입력`하여 만듭니다.
> `플레이 맵`에 필수 필드인 `startArea`와 `goalArea` 등에 사용됩니다.

### + TODO

.  
.  
.  
.  
.  
.  
.  
.  
.  
.  
.  
.  
.  
.  
.  
.  
.  
.  