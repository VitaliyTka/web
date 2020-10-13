using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeeRegistration;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using EmployeeRegistration.Models;
using System.IO;

namespace EmployeeRegistration.Controllers
{

    public class PicturesController : Controller
    {
        public static Cloudinary cloudinary;
        public const string cloud_name = "ddsvnrupg";
        public const string api_key = "213771544495596";
        public const string api_secret = "a9lxmo_tH-Gl9t7BouRtZy8Z9eU";

        IWebHostEnvironment _appEnvironment;

        private readonly DBWorkersContext _context;

        public PicturesController(DBWorkersContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        // GET: Pictures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Picture.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Pictures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture
                .FirstOrDefaultAsync(m => m.Id == id);
            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        // GET: Pictures/Create
        public IActionResult Create()
        {
            return View();
        }
        public static ImageUploadResult uploadIMG(string imgPath)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imgPath),
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult;
        }
        // POST: Pictures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PicturePath")] Picture picture, IFormFile uploadedFile)
        {
            //"C:\\Users\\VITALIY\\source\repos\\EmployeeRegistration\\EmployeeRegistration\\wwwroot\\img\\"s
            string pathCloud = "";
            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/files/" + uploadedFile.FileName;
                pathCloud = path;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
            }
            pathCloud = _appEnvironment.WebRootPath + pathCloud;
            Account account = new Account(cloud_name, api_key, api_secret);
            cloudinary = new Cloudinary(account);
            var uploadResult = uploadIMG(pathCloud);
            
            //Console.WriteLine("asdasd " + picture.PicturePath);
            /*
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(@"C:\Users\VITALIY\source\repos\EmployeeRegistration\EmployeeRegistration\wwwroot\img\" + picture.PicturePath),
                PublicId = "Home/workers/" + picture.PicturePath,
                Overwrite = true,
                NotificationUrl = "https://mysite/my_notification_endpoint"
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            */

            picture.PicturePath = uploadResult.Uri.ToString();
            if (ModelState.IsValid)
            {
                _context.Add(picture);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(picture);
        }

        // GET: Pictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture.FindAsync(id);
            if (picture == null)
            {
                return NotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PicturePath")] Picture picture)
        {
            if (id != picture.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(picture);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PictureExists(picture.Id))
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
            return View(picture);
        }

        // GET: Pictures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var picture = await _context.Picture
                .FirstOrDefaultAsync(m => m.Id == id);
            if (picture == null)
            {
                return NotFound();
            }

            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var picture = await _context.Picture.FindAsync(id);
            _context.Picture.Remove(picture);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PictureExists(int id)
        {
            return _context.Picture.Any(e => e.Id == id);
        }
    }
}
