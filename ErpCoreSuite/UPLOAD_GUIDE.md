# 🚀 How to Upload This Project to GitHub
## (Step-by-step for syed-sulaiman-dev)

---

## STEP 1 — Install Git
Download from: https://git-scm.com/download/win
Install with default settings. Then open **Command Prompt**.

---

## STEP 2 — Configure Git (one time only)
```
git config --global user.name "Syed Sulaiman"
git config --global user.email "Sulaimanjaila91@gmail.com"
```

---

## STEP 3 — Create Repo on GitHub
1. Go to github.com → Login as syed-sulaiman-dev
2. Click **"+"** → **New repository**
3. Name: `erp-core-suite`
4. Description: `Full-stack ERP demo — ASP.NET Core · SQL Server · Bootstrap 5`
5. Set to **Public**
6. Do NOT check "Add README" (we already have one)
7. Click **Create repository**

---

## STEP 4 — Copy Project Folder
Copy the downloaded ErpCoreSuite folder to:
```
C:\Projects\ErpCoreSuite\
```

---

## STEP 5 — Open Command Prompt in That Folder
```
cd C:\Projects\ErpCoreSuite
```

---

## STEP 6 — Push to GitHub
```bash
git init
git add .
git commit -m "Initial commit: ERP Core Suite - Inventory, Payroll & Accounting"
git branch -M main
git remote add origin https://github.com/syed-sulaiman-dev/erp-core-suite.git
git push -u origin main
```

GitHub will ask for your username and password (use your GitHub password or Personal Access Token).

---

## STEP 7 — Done! ✅
Visit: https://github.com/syed-sulaiman-dev/erp-core-suite

Your project is now live on GitHub! 🎉

---

## To Add to Your CV
In your CV under a new "Projects" section, write:

**ERP Core Suite** | github.com/syed-sulaiman-dev/erp-core-suite
- Full-stack ERP demo system with Inventory, Payroll & Accounting modules
- Stack: C# · ASP.NET Core 6 · SQL Server · Bootstrap 5 · JWT Auth
