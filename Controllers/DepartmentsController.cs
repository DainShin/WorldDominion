using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorldDominion.Models;

namespace WorldDominion.Controllers
{
    public class DepartmentsController : Controller
    {
        // _context 변수선언 : DbContext를 저장하기 위한 필드
        // readonly: 생성자에서만 값 변경 가능
        private readonly ApplicationDbContext _context; 

        // DepartmentsController 클래스 생성자
        // ApplicationDbContext 타입의 파라미터를 받아 _context 필드 초기화
        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Departments

        // Department목록을 나타내는 페이지를 렌더링. DB에서 Department 목록을 불러와 뷰로 전달
        // async : 비동기 작업을 위한 키워드
        // Task : 비동기적으로 수행되어야 하는 작업을 나타냄
        // Task<int>는 int값을 반환하는 비동기 작업
        // IActionResult는 컨트롤러의 액션 메서드가 처리한 결과를 나타내는 인터페이스. HTTP응답을 나타내며 웹요청에 대한 적절한 응답을 생성하여 클라이언트에게 반환
        public async Task<IActionResult> Index()
        {
              return _context.Departments != null ? 
                          View(await _context.Departments.ToListAsync()) : // Departments 테이블에서 모든 레코드를 비동기적으로 읽어와서 리스트로 만든후, View로 전달
                          Problem("Entity set 'ApplicationDbContext.Departments'  is null.");
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            // Entity Framework : sql 문
            var department = await _context.Departments // 비동기적으로 데이터 조회
                // Accessing the department's table and selecting all the departments
                // Include: join. Products 테이블과 조인한 후 OrderBy 사용. Products 테이블을 Name속성 기준으로 정렬
                .Include(department =>  department.Products.OrderBy(product => product.Name))
                .FirstOrDefaultAsync(m => m.Id == id);  // SELECT * FROM departments WHERE Id = id;
            if (department == null)
            {
                return NotFound();
            }

            return View(department); // department는 _context.Departments에서 쿼리를 통해 얻어온 데이터
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Department department)
        {
            if (id != department.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Departments == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Departments'  is null.");
            }
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
          return (_context.Departments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
