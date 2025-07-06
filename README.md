# Who Wants to Be a Millionaire - WinForms Edition

A simple Windows Forms game inspired by "Who Wants to Be a Millionaire", where players answer AI-generated multiple-choice questions to climb the prize ladder.

## 🧠 Features
- Multiple-choice trivia questions
- AI-generated questions using Google Gemini API
- Difficulty levels: Easy, Medium, Hard

## 🚀 Setup Instructions

1. **Clone the repository**:
   ```bash
   git clone https://github.com/ItzzAnnemos/MillionaireQuiz.git
   cd millionaire-game-winforms
   
2. **Open the solution in Visual Studio**:
   - Double-click `MillionaireQuiz.sln` to open the project.
   - Ensure you have the appropriate version of **.NET Framework** (or **.NET Core**) installed.

3. **Run the project**:
   - Click the **Start** button or press `F5` in Visual Studio to build and run the game.


## Documentation

># Подетално упатсво и слики се достапни во прикачениот документ. [Millionaire Quiz - VP.docx](./Millionaire%20Quiz%20-%20VP.docx)

**Authors:**
- Мартин Розин Сопков 223222  
- Никола Серафимов 223125  
- Христијан Стоимилов 223163  

---

## 1. Опис на апликацијата

„Millionaire Quiz“ е едукативно-забавна апликација развиена како Windows Forms игра со цел да се симулира квизот од популарната ТВ-емисија “Who Wants to Be a Millionaire?”. 

Апликацијата служи како интерактивен начин за тестирање и унапредување на знаењето на корисникот преку забавен формат со прашања и награди.

### Главни карактеристики:
- 15 прашања со растечка тежина (Easy, Medium, Hard)
- Виртуелна добивка до 1.000.000 евра
- Постепен напредок со секое точно прашање
- 3 типа на **помошни алатки (lifelines)**:
  - **50/50** – Елиминира два неточни одговори
  - **Call a Friend** – Симулира одговор од пријател
  - **Ask the Audience** – Симулира одговор од публика
- Секоја помош се користи само еднаш

Апликацијата е наменета за сите возрасни групи и може да се користи како алатка за учење, забава или пријателско натпреварување.

---

## 2. Решение на проблемот

Апликацијата е развиена во **C# со .NET Framework**, користејќи **Windows Forms** за GUI.

### Структура на податоци:
Класа `Question` содржи:
- `Question` – текст на прашањето  
- `Answers[]` – низа од 4 можни одговори  
- `Correct` – точен одговор  
- `Difficulty` – тежина (Easy, Medium, Hard)  

---

## 3. Опис на функција/класа

### Метода: `button5_Click`

Оваа функција се поврзува со клик на копчињата за помош:
- `callFriendButton`
- `askAudienceButton`

#### Функционалност:
- Се симулира одговор од пријател/публика
- Веројатност за точен одговор според тежината:
  - Easy: 90%
  - Medium: 70%
  - Hard: 50%
- Ако случајниот број е под веројатноста → враќа точен одговор
- Инаку → случаен неточен одговор
- Прикажува порака со предлог-одговор
- Копчето се оневозможува по употреба

---

## 4. Скриншоти и кратко упатство за користење

### Подетално упатство и слики во прикачениот word документ.

### Упатство:
- Апликацијата стартува директно со првото прашање
- Играчот избира еден од 4 одговори
- Ако одговорот е точен → продолжува понатаму
- Ако е неточен → играта завршува
- Може да се користат 3 помошни алатки (само еднаш)

---

## 5. Опис на користење на вештачка интелигенција – **Google Gemini**

### Класа: `GenerateQuestion`

Одговорна за автоматско генерирање на прашања преку **Google Gemini AI**.

#### Како функционира:

1. **Подесување на API**
   - `ApiKey` за пристап до Gemini моделот
   - `Endpoint` URL за повик

2. **Генерирање на прашање**
   - Избор на случајна категорија (нпр. Географија)
   - Креирање на `prompt` со упатства (1 прашање, 4 одговори, 1 точен)

3. **Форматирање на JSON барање**
   - Се испраќа во формат што Gemini го разбира
   - `temperature`, `topK`, `topP` – контролираат креативност

4. **Извршување на API повик**
   - Испраќа барање
   - Прима одговор

5. **Парсирање на одговорот**
   - Извлекување на текстот со прашањето и одговорите

6. **Ракување со грешки**
   - Ако одговорот е невалиден или празен → се прикажува порака со грешка

---

