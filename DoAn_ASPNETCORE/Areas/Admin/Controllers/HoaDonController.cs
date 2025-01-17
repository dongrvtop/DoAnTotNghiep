﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAn_ASPNETCORE.Areas.Admin.Data;
using DoAn_ASPNETCORE.Areas.Admin.Models;

namespace DoAn_ASPNETCORE.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonController : Controller
    {
        private readonly Webbanhang _context;

        public HoaDonController(Webbanhang context)
        {
            _context = context;
        }

        // GET: Admin/HoaDon
        //public async Task<IActionResult> Index(string SearchString)
        //{
        //    IQueryable<string> genreQuery = from m in _context.HoaDonModel
        //                                    select m.HoTen;

        //    var HoaDon = from m in _context.HoaDonModel
        //                            select m;

        //    if (!string.IsNullOrEmpty(SearchString))
        //    {
        //        HoaDon = HoaDon.Where(s => s.HoTen.Contains(SearchString));
        //    }



        //    var HoaDonViewModel = new HoaDonViewModel
        //    {
        //        HD = new SelectList(await genreQuery.Distinct().ToListAsync()),
        //        HoaDons = await HoaDon.ToListAsync()
        //    };

        //    return View(HoaDonViewModel);

        //}
        public async Task<IActionResult> Index()
        {
            ViewData["User_ID"] = new SelectList(_context.Set<UserModel>(), "ID", "UserName");
            return View();
        }


        // GET: Admin/HoaDon/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonModel = await _context.HoaDonModel
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hoaDonModel == null)
            {
                return NotFound();
            }

            return View(hoaDonModel);
        }

        // GET: Admin/HoaDon/Create
        public IActionResult Create()
        {
            ViewData["User_ID"] = new SelectList(_context.Set<UserModel>(), "ID", "ID");
            return View();
        }

        // POST: Admin/HoaDon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,User_ID,HoTen,Sdt,DiaChi,ThanhTien,TrangThai,ListDetail")] HoaDonModel hoaDonModel, /*[Bind("ID,HoaDon_ID,TenSP,SoLuong,Gia,KhuyenMai,ThanhTien,TrangThai")] ChiTietHoaDonModel[] LstchitiethoaDonModel*/ string[] TenSP, string[] SoLuong, string[] Gia, int[] TrangThai)
        {
            var url = Url.RouteUrl(new { area = "", controller = "Pages", action = "Index" });
            try
            {
                var HoaDon = from m in _context.HoaDonModel
                             select m;
                int size = HoaDon.Count();
                //if (ModelState.IsValid)
                //{
                _context.Add(hoaDonModel);
                size++;
                await _context.SaveChangesAsync();
                for (var i = 0; i < TenSP.Count(); i++)
                {
                    var CTDH = new ChiTietHoaDonModel();
                    CTDH.HoaDon_ID = hoaDonModel.ID;
                    CTDH.TenSP = TenSP[i];
                    CTDH.SoLuong = SoLuong[i].Contains("/") ? SoLuong[i].Split("/")[0] : SoLuong[i];
                    CTDH.Gia = Gia[i];
                    CTDH.TrangThai = TrangThai[i];
                    CTDH.ThanhTien = Int32.Parse(Gia[i]) * Int32.Parse(SoLuong[i].Contains("/") ? SoLuong[i].Split("/")[0] : SoLuong[i]);
                    _context.Add(CTDH);
                    await _context.SaveChangesAsync();
                }
                //chitiethoaDonModel.HoaDon_ID = hoaDonModel.ID;
                //foreach(var item in LstchitiethoaDonModel)
                //{
                //    item.HoaDon_ID = hoaDonModel.ID;
                //    _context.Add(item);
                //    await _context.SaveChangesAsync();
                //}
                HttpContext.Session.Remove("cart");
                return Redirect(url);
                //}
                //ViewData["User_ID"] = new SelectList(_context.Set<UserModel>(), "ID", "ID", hoaDonModel.User_ID);
                //return View(hoaDonModel);
            }
            catch (Exception ex)
            {
                //throw ex;
                return Redirect(url);
            }
            
        }

        // GET: Admin/HoaDon/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonModel = await _context.HoaDonModel.FindAsync(id);
            if (hoaDonModel == null)
            {
                return NotFound();
            }
            ViewData["User_ID"] = new SelectList(_context.Set<UserModel>(), "ID", "ID", hoaDonModel.User_ID);
            return View(hoaDonModel);
        }

        // POST: Admin/HoaDon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,User_ID,HoTen,Sdt,ThanhTien,TrangThai")] HoaDonModel hoaDonModel)
        {
            if (id != hoaDonModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDonModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonModelExists(hoaDonModel.ID))
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
            ViewData["User_ID"] = new SelectList(_context.Set<UserModel>(), "ID", "ID", hoaDonModel.User_ID);
            return View(hoaDonModel);
        }

        // GET: Admin/HoaDon/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDonModel = await _context.HoaDonModel
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (hoaDonModel == null)
            {
                return NotFound();
            }

            return View(hoaDonModel);
        }

        // POST: Admin/HoaDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDonModel = await _context.HoaDonModel.FindAsync(id);
            _context.HoaDonModel.Remove(hoaDonModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonModelExists(int id)
        {
            return _context.HoaDonModel.Any(e => e.ID == id);
        }
    }
}
