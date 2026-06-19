# FPS Game System

완벽하게 설정된 FPS 게임 시스템입니다.

## 실행 방법

### Unity 에디터에서
1. Unity 6000.0.32f1 이상에서 이 프로젝트 폴더 열기
2. Assets/Scenes/Game.unity 씬 열기
3. Play 버튼 클릭

### 웹 브라우저에서 (WebGL 자동 빌드)
`main` 브랜치에 push되면 GitHub Actions가 WebGL로 빌드하여 GitHub Pages에 자동 배포합니다.
이 기능을 사용하려면 저장소 Settings → Secrets에 `UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`를 등록하고,
Settings → Pages에서 Source를 "GitHub Actions"로 설정해야 합니다.

## 조작

- **마우스**: 시점 이동
- **WASD**: 이동
- **Space**: 점프
- **Shift**: 달리기
- **마우스 좌클릭**: 사격
- **마우스 우클릭**: 조준
- **R**: 재장전
- **1, 2, 3**: 무기 선택
- **ESC**: 마우스 잠금 해제

## 주요 시스템

- ✅ 무기 시스템 (사격, 재장전)
- ✅ 반동 시스템
- ✅ 피해 시스템 (헤드샷, 다리 피해)
- ✅ 라운드 시스템
- ✅ 팀 시스템
- ✅ UI 시스템

## 파일 구조

```
Assets/
├── Scripts/          # 모든 게임 로직
├── Scenes/
│   └── Game.unity    # 메인 씬
├── Prefabs/          # 게임 객체 템플릿
└── Resources/        # 리소스 파일
```
