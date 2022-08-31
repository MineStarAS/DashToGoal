## 플레이어 (Player)

- [컨셉 설명 링크](../../Concept.md#플레이어-(Player))

---

### [필드](../../DescriptionRecipe.md#필드-작성법)

> #### 상수
> - name(`String`)
> - passiveSkill(`PacciveSkill`)
> - activeSkill1(`ActiveSkill`)
> - activeSkill2(`ActiveSkill`)
> - _effects (`Map<Effect>`)

> #### 변수
> - _moveForce (`Float`, `super`)
> - _jumpForce (`Float`, `super`)
> - _item (`Item`, `nullable`)
> - _airJump (`Byte`)

---

### [메소드](../../DescriptionRecipe.md#메소드-작성법)

> - getMoveForce(): `Float`  
> 
> `_moveForce`를 `_effects`로 모두 연산 후 출력합니다.

> - getJumpForce(): `Float`  
> 
> `_jumpForce`를 `_effects`로 모두 연산 후 출력합니다.

> - doJump(): `void`  
> 
> `플레이어`가 공중에 있는지 없는지 확인 후  
> `플레이어`에게 `getJumpForce`만큼의 `Vector`를 부여합니다.
> ```kotlin
> if (isAir()) {
>   if (_airJump <= 0) return;
>   _airJump--;
> }
> ```
---

### 애니메이션 (Animation)

> #### 대기

> #### 달리기

> #### 점프

> #### 공중

> #### 기절

---

### 구상된 플레이어

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