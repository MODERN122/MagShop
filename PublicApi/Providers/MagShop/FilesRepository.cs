
//using Amazon.S3;
//using Amazon.S3.Model;
//using Microsoft.Extensions.Primitives;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;

//namespace PublicApi.Providers.MagShop
//{                
//        public sealed class FilesRepository
//        {

//            public bool FileDelete(int fileId, string ownManLogin)
//            {
//                //var cloudDeletedSuccess = Providers.TsCloud.Provider.FileDelete(req.Item2.Path);

//                var ownManId = UsersRepository.OwnManId(ownManLogin);

//                var fileDb = Fastzila.Files.Where(_ => _.Id == fileId).FirstOrDefault();
//                if (fileDb != null)
//                {
//                    fileDb.IsDeleted = true;
//                    fileDb.DeletedBy = ownManId;
//                    fileDb.Deleted = DateTime.Now;
//                    db.Update(fileDb);
//                    return true;
//                }
//                else
//                {
//                    throw new InvalidDataException("Файл с таким id в базе не найден!");
//                }
//            }

//            public async Task<Files.File> FileUpload(Stream stream, string extension,
//               string title, Files.FileObject objType, Files.FileType fileType, int objId, int objIdAdd,
//               string ownManLogin)
//            {
//                stream.Position = 0;
//                var ownManId = UsersRepository.OwnManId(ownManLogin);
//                var cloudHash = await AwsS3.Provider.UploadFromStream(stream, extension, String.Format("{0}/{1}",
//                   Files.FileObjectDict.GetValueOrDefault(objType, "undefined"), Files.FileTypeDict.GetValueOrDefault(fileType, "unefined")));
//                var file = new Files.File
//                {
//                    Id = -1,
//                    Created = DateTime.Now,
//                    CreatedBy = ownManId,
//                    CreatorName = "",
//                    FileType = fileType,
//                    ObjectId = objId,
//                    ObjectIdAdd = objIdAdd,
//                    ObjectType = objType,
//                    Path = cloudHash,
//                    Title = title
//                };

//                return AddFile(file, ownManLogin);
//            }

//            public Files.File[] SearchFiles(Spec filesFilter)
//            {
//                var files = Files.Filter(filesFilter);

//                var creatorsDict = Users.Where(_ => files.Any(f => f.CreatedBy == _.Id))
//                   .ToDictionary(_ => _.Id, _ => _.Surname + " " + _.Name);

//                return files.Select(_ => ConvToModel(_, creatorsDict.GetValueOrDefault(_.CreatedBy, "Не найден"))).ToArray();
//            }

//            public Domain.Files.FilesInventory SearchWithInv(Spec filesFilter)
//            {
//                var files = Fastzila.Files.Filter(filesFilter);

//                var creatorsDict = Fastzila.Users.Where(_ => files.Any(f => f.CreatedBy == _.Id))
//                   .ToDictionary(_ => _.Id, _ => _.Surname + " " + _.Name);

//                var filesResult = files.Select(_ => ConvToModel(_, creatorsDict.GetValueOrDefault(_.CreatedBy, "Не найден"))).ToArray();

//                var users = files.Where(_ => _.ObjectType == (int)FileObject.OwnManager).ToArray()
//                                 .GroupBy(_ => _.ObjectId).Select(_ => _.Key).ToArray();
//                var userEntries = Fastzila.Users.Where(_ => users.Contains(_.Id))
//                                  .Select(_ => new Entry() { Id = _.Id, Name = "[" + _.Id + "] " + _.Surname + _.Name + _.Patronymic }).ToArray();


//                var doers = files.Where(_ => _.ObjectType == (int)FileObject.Doer).ToArray()
//                                 .GroupBy(_ => _.ObjectId).Select(_ => _.Key).ToArray();
//                var doerEntries = Fastzila.Couriers.Where(_ => doers.Contains(_.Id))
//                                   .Select(_ => new Entry() { Id = _.Id, Name = "[" + _.Id + "] " + _.Fullname }).ToArray();


//                var leads = files.Where(_ => _.ObjectType == (int)FileObject.Lead).ToArray()
//                                 .GroupBy(_ => _.ObjectId).Select(_ => _.Key).ToArray();
//                var leadEntries = Fastzila.Leads.Where(_ => leads.Contains(_.Id))
//                                   .Select(_ => new Entry() { Id = _.Id, Name = "[" + _.Id + "] " + _.Surname + _.Name + _.Patronymic }).ToArray();


//                var contacts = files.Where(_ => _.ObjectType == (int)FileObject.Contact).ToArray()
//                                .GroupBy(_ => _.ObjectId).Select(_ => _.Key).ToArray();
//                var contactEntries = Fastzila.Contacts.Where(_ => contacts.Contains(_.Id))
//                                     .Select(_ => new Entry() { Id = _.Id, Name = "[" + _.Id + "] " + _.Surname + _.Name + _.Patronymic }).ToArray();

//                var turns = files.Where(_ => _.ObjectType == (int)FileObject.Turn).ToArray()
//                                 .GroupBy(_ => _.ObjectId).Select(_ => _.Key).ToArray();
//                var turnEntries = Fastzila.Turns.Where(_ => turns.Contains(_.Id))
//                                     .Select(_ => new Entry() { Id = _.Id, Name = "[" + _.Id + "]" }).ToArray();


//                var doerFeedComments = files.Where(_ => _.ObjectType == (int)FileObject.DoerFeedComment).ToArray()
//                                       .GroupBy(_ => _.ObjectId).Select(_ => _.Key).ToArray();
//                var doerFeedCommentNode = Fastzila.DoerFeedComments.Where(_ => doerFeedComments.Contains(_.Id))
//                                             .Select(_ => new Node(_.Id, "[" + _.Id + "]", _.FeedId)).ToArray();

//                var entities = files.Where(_ => _.ObjectType == (int)FileObject.Entity).ToArray()
//                        .GroupBy(_ => _.ObjectId).Select(_ => _.Key).ToArray();
//                var entityEntries = Fastzila.Entities.Where(_ => entities.Contains(_.Id))
//                                   .Select(_ => new Entry() { Id = _.Id, Name = "[" + _.Id + "] " + _.FullName }).ToArray();

//                return new Domain.Files.FilesInventory()
//                {
//                    Files = filesResult,
//                    DoerFeedComments = doerFeedCommentNode,
//                    Users = userEntries,
//                    Doers = doerEntries,
//                    Leads = leadEntries,
//                    Contacts = contactEntries,
//                    Turns = turnEntries,
//                    Entities = entityEntries
//                };

//            }

//            public async Task<Files.File[]> SearchFilesAsync(Spec filesFilter)
//            {
//                var files = Fastzila.Files.Filter(filesFilter);

//                var creatorsDict = await Fastzila.Users.Where(_ => files.Any(f => f.CreatedBy == _.Id))
//                   .ToDictionaryAsync(_ => _.Id, _ => _.Surname + " " + _.Name);

//                return await files.Select(_ => ConvToModel(_, creatorsDict.GetValueOrDefault(_.CreatedBy, "Не найден"))).ToArrayAsync();
//            }

//            public Files.File AddFile(Files.File fm, string ownManLogin)
//            {
//                var ownManId = UsersRepository.OwnManId(ownManLogin);

//                var newFile = new File
//                {
//                    Title = fm.Title,
//                    ObjectType = (int)fm.ObjectType,
//                    ObjectId = fm.ObjectId,
//                    ObjectIdAdd = fm.ObjectIdAdd,
//                    Path = fm.Path,
//                    FileType = (int)fm.FileType,
//                    Created = fm.Created,
//                    CreatedBy = fm.CreatedBy
//                };
//                var newFileId = db.InsertWithInt32Identity(newFile);
//                newFile.Id = newFileId;

//                var creatorName = Fastzila.Users.Where(i => i.Id == ownManId)
//                   .Select(i => i.Surname + " " + i.Name).FirstOrDefault() ?? "Не найден";

//                return ConvToModel(newFile, creatorName);
//            }

//            public static Files.File ConvToModel(File _, string creatorName)
//            {
//                return new Files.File
//                {
//                    Id = _.Id,
//                    Path = _.Path,
//                    Title = _.Title,
//                    ObjectType = (Files.FileObject)_.ObjectType,
//                    ObjectId = _.ObjectId,
//                    ObjectIdAdd = _.ObjectIdAdd,
//                    FileType = (Files.FileType)_.FileType,
//                    Created = _.Created,
//                    CreatedBy = _.CreatedBy,
//                    CreatorName = creatorName
//                };
//            }

//            public static FastzilaDBNS.File ConvToDb(Files.File _, bool isDeleted = false, int deletedBy = 0, DateTime? deleted = null)
//            {
//                return new FastzilaDBNS.File
//                {
//                    Id = _.Id,
//                    Path = _.Path,
//                    Title = _.Title,
//                    ObjectType = (int)_.ObjectType,
//                    ObjectId = _.ObjectId,
//                    ObjectIdAdd = _.ObjectIdAdd,
//                    FileType = (int)_.FileType,
//                    Created = _.Created,
//                    CreatedBy = _.CreatedBy,
//                    IsDeleted = isDeleted,
//                    DeletedBy = deletedBy,
//                    Deleted = deleted
//                };
//            }
//        }
//    }